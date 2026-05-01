namespace OrdersService.Core.Events.Models;

/// <summary>
/// Base interface for domain events
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Type of Domain Event
    /// </summary>
    public string Type { get; }
    
    /// <summary>
    /// Gets the partition key for the event
    /// </summary>
    /// <returns></returns>
    string GetPartitionKey();
}