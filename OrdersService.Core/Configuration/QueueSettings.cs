namespace OrdersService.Core.Configuration;

/// <summary>
/// Configuration settings for message queue operations.
/// </summary>
public class QueueSettings
{
    /// <summary>
    /// Configuration section name for queue settings.
    /// </summary>
    public const string SectionName = "QueueSettings";

    /// <summary>
    /// Name of the queue to use for message exchange.
    /// </summary>
    public required string QueueName { get; set; }
}