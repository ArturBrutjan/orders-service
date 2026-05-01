using OrdersService.Infrastructure.Rabbit.Configuration;
using RabbitMQ.Client;

namespace OrdersService.Infrastructure.Rabbit.Connection;

/// <inheritdoc />
public class RabbitMqConnectionBuilder : IRabbitMqConnectionBuilder
{
    /// <inheritdoc />
    public Task<IConnection> GetConnectionAsync(RabbitMqOptions options, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.Host,
            UserName = options.Username,
            Password = options.Password,
            Port = options.Port,
            VirtualHost = options.VirtualHost,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        return factory.CreateConnectionAsync(cancellationToken);
    }
}