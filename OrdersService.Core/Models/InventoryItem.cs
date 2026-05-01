namespace OrdersService.Core.Models;

/// <summary>
/// Represents an item in the inventory with its unique identifier, price, and available quantity.
/// </summary>
public record InventoryItem
{
    /// <summary>
    /// Unique identifier for the item in the inventory.
    /// </summary>
    public required Guid InventoryId { get; init; }
    
    /// <summary>
    /// The price of the item in the inventory.
    /// </summary>
    public required decimal Price { get; set; }
}