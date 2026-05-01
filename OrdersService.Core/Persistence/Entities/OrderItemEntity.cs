namespace OrdersService.Core.Persistence.Entities;

/// <summary>
/// Represents an order item entity with properties for item id, price, and quantity.
/// </summary>
public class OrderItemEntity
{
    /// <summary>
    /// Order item id
    /// </summary>
    public required Guid OrderItemId { get; init; }
    
    /// <summary>
    /// Order item price
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Order item quantity
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    public required Guid OrderId { get; set; }

    /// <summary>
    /// Inventory item id
    /// </summary>
    public required Guid InventoryItemId { get; set; }

    /// <summary>
    /// Single inventory item price
    /// </summary>
    public decimal? SingleItemPrice { get; set; }

    /// <summary>
    /// Subtotal of the order item
    /// </summary>
    public decimal? Subtotal { get; set; }
}