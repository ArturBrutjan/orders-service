using OrdersService.Core.Models;

namespace OrdersService.Core.Persistence;

/// <summary>
/// Provides methods for managing inventory items in the system.
/// </summary>
public interface IInventoryRepository : IRepository
{
    /// <summary>
    /// Retrieves a list of inventory items by their unique identifiers.
    /// </summary>
    /// <param name="inventoryIds"></param>
    /// <returns></returns>
    Task<List<InventoryItem>> GetInventoryItemsByIdsAsync(List<Guid> inventoryIds);
}