namespace OrdersService.Core.Persistence.Entities;

/// <summary>
/// Outbox Message State
/// </summary>
public enum MessageState
{
    /// <summary>
    /// Message is new
    /// </summary>
    New,
    
    /// <summary>
    /// Message has been processed
    /// </summary>
    Processed
}