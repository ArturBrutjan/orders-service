namespace OrdersService.Core.Events.Models;

/// <summary>
/// Order Created Event
/// </summary>
public class OrderCreatedEvent : IDomainEvent
{
    /// <summary>
    /// Event Type for Order Created Event
    /// </summary>
    public const string EventType = "com.mybestcompany.orders.created";
    
    /// <inheritdoc />
    public string Type => EventType;

    /// <summary>
    /// Order Id
    /// </summary>
    public required Guid OrderId { get; set; }

    /// <summary>
    /// Order Items
    /// </summary>
    public required List<OrderItemsEventPayload> Items { get; set; }

    /// <inheritdoc />
    public string GetPartitionKey() => OrderId.ToString();
}

/// <summary>
/// Order Items Event Payload
/// </summary>
public class OrderItemsEventPayload
{
    /// <summary>
    /// Order Item Id
    /// </summary>
    public Guid OrderItemId { get; set; }

    /// <summary>
    /// Order Item Quantity
    /// </summary>
    public int Amount { get; set; }
}