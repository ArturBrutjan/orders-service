namespace OrdersService.Core.Models;

/// <summary>
/// Order Item domain model
/// </summary>
public record OrderItem
{
    /// <summary>
    /// Item Id
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Order Id
    /// </summary>
    public required Guid OrderId { get; set; }

    /// <summary>
    /// Inventory Item Id
    /// </summary>
    public required Guid InventoryItemId { get; init; }
    
    /// <summary>
    /// Item Quantity
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Item Price
    /// </summary>
    public decimal? SingleItemPrice { get; set; }

    /// <summary>
    /// Item Subtotal
    /// </summary>
    public decimal? Subtotal { get; set; }
}