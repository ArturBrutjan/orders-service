using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrdersService.Core.Configuration;
using OrdersService.Core.Persistence;
using OrdersService.Core.Persistence.Entities;
using OrdersService.Infrastructure.Rabbit.Configuration;
using OrdersService.Infrastructure.Rabbit.Connection;
using RabbitMQ.Client;

namespace OrdersService.Infrastructure.Rabbit.Workers;

/// <summary>
/// RabbitMq Producer Service
/// </summary>
public class RabbitMqProducerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IRabbitMqConnectionBuilder _builder;
    private readonly IOptions<RabbitMqOptions> _rabbitConfig;
    private readonly string _queueName;
    private readonly ILogger<RabbitMqProducerService> _logger;

    private const int BatchSize = 20;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="scopeFactory"></param>
    /// <param name="builder"></param>
    /// <param name="rabbitConfig"></param>
    /// <param name="queueSettings"></param>
    /// <param name="logger"></param>
    public RabbitMqProducerService(IServiceScopeFactory scopeFactory,
        IRabbitMqConnectionBuilder builder,
        IOptions<RabbitMqOptions> rabbitConfig,
        IOptions<QueueSettings> queueSettings,
        ILogger<RabbitMqProducerService> logger)
    {
        _scopeFactory = scopeFactory;
        _rabbitConfig = rabbitConfig;
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
        _logger.LogInformation("Starting RabbitMq producer service");

        await using var connection = await _builder.GetConnectionAsync(_rabbitConfig.Value, stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false,
            cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

            await unitOfWork.BeginTransactionAsync();

            var messages = await outboxRepository.GetOutboxMessageBatchAsync(BatchSize, stoppingToken);

            if (!messages.Any())
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            foreach (var message in messages)
            {
                _logger.LogInformation("Publishing message with id {MessageId}", message.MessageId);

                var body = message.Payload;

                var properties = new BasicProperties
                {
                    Persistent = true,
                    ContentType = "application/json",
                    Type = message.Type
                };

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: _queueName,
                    mandatory: true,
                    body: body,
                    basicProperties: properties,
                    cancellationToken: stoppingToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
                message.State = MessageState.Processed;
                await outboxRepository.UpdateOutboxMessageAsync(message);
            }

            await unitOfWork.CommitAsync();
            await Task.Delay(1000, stoppingToken);
        }
    }
}