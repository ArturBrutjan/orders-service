using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrdersService.Core.Configuration;
using OrdersService.Core.Events;
using OrdersService.Infrastructure.Rabbit.Configuration;
using OrdersService.Infrastructure.Rabbit.Connection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrdersService.Infrastructure.Rabbit.Workers;

/// <summary>
/// RabbitMQ Consumer Service
/// </summary>
[SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created")]
public class RabbitMqConsumerService : BackgroundService
{
    private readonly EventHandlerRegistry _registry;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRabbitMqConnectionBuilder _builder;
    private readonly RabbitMqOptions _rabbitConfig;
    private readonly string _queueName;
    private readonly ILogger<RabbitMqConsumerService> _logger;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="registry"></param>
    /// <param name="scopeFactory"></param>
    /// <param name="builder"></param>
    /// <param name="rabbitConfig"></param>
    /// <param name="queueSettings"></param>
    /// <param name="logger"></param>
    public RabbitMqConsumerService(EventHandlerRegistry registry,
        IServiceScopeFactory scopeFactory,
        IRabbitMqConnectionBuilder builder,
        IOptions<RabbitMqOptions> rabbitConfig,
        IOptions<QueueSettings> queueSettings,
        ILogger<RabbitMqConsumerService> logger)
    {
        _registry = registry;
        _scopeFactory = scopeFactory;
        _rabbitConfig = rabbitConfig.Value;
        _logger = logger;
        _builder = builder;
        _queueName = queueSettings.Value.QueueName;
    }

    /// <summary>
    /// Event processing executioner
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting RabbitMQ consumer service");
        var connection = await _builder.GetConnectionAsync(_rabbitConfig, stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            // We assume the event type is passed in the message properties
            var eventType = ea.BasicProperties.Type;
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());

            if (string.IsNullOrEmpty(eventType))
            {
                _logger.LogWarning("Received message without a type.");
                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                return;
            }

            var handlerType = _registry.GetHandlerType(eventType);
            if (handlerType == null)
            {
                _logger.LogWarning("No handler registered for event type: {Type}", eventType);
                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            try
            {
                _logger.LogInformation("Handling event {Type}", eventType);
                var handler = (IEventHandler)scope.ServiceProvider.GetRequiredService(handlerType);
                await handler.HandleAsync(body);

                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event {Type}", eventType);
                await channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        _logger.LogInformation("RabbitMQ consumer started for queue: {QueueName}", _queueName);
    }
}