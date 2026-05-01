using System.Diagnostics.CodeAnalysis;

namespace OrdersService.Infrastructure.Rabbit.Configuration;

/// <summary>
/// Configuration options for RabbitMQ integration.
/// </summary>
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class RabbitMqOptions
{
    /// <summary>
    /// Section Name
    /// </summary>
    public const string SectionName = "RabbitMQ";
    
    /// <summary>
    /// RabbitMQ host name
    /// </summary>
    public required string Host { get; init; }
    
    /// <summary>
    /// RabbitMQ user name
    /// </summary>
    public required string Username { get; init; }
    
    /// <summary>
    /// RabbitMQ user password
    /// </summary>
    public required string Password { get; init; }
    
    /// <summary>
    /// RabbitMQ virtual host
    /// </summary>
    public string VirtualHost { get; init; } = "/";
    
    /// <summary>
    /// 
    /// </summary>
    public int Port { get; init; } = 5672;
}