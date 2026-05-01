namespace OrdersService.Core.Persistence.Entities;

/// <summary>
/// Represents the inventory details for an item.
/// </summary>
public class InventoryEntity
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