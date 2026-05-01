using OrdersService.Infrastructure.Rabbit.Configuration;
using RabbitMQ.Client;

namespace OrdersService.Infrastructure.Rabbit.Connection;

/// <summary>
/// RabbitMq Connection Builder
/// </summary>
public interface IRabbitMqConnectionBuilder
{
    /// <summary>
    /// Get RabbitMq Connection
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IConnection> GetConnectionAsync(RabbitMqOptions options, CancellationToken cancellationToken = default);
}