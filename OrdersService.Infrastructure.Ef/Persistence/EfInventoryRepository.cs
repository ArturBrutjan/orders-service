using Microsoft.EntityFrameworkCore;
using OrdersService.Core.Models;
using OrdersService.Core.Persistence;

namespace OrdersService.Infrastructure.Ef.Persistence;

/// <summary>
/// Implementation of IInventoryRepository using Entity Framework Core.
/// </summary>
public class EfInventoryRepository : IInventoryRepository
{
    private readonly OrdersServiceDbContext _context;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="context"></param>
    public EfInventoryRepository(OrdersServiceDbContext context) => _context = context;

    /// <inheritdoc />
    public Task<List<InventoryItem>> GetInventoryItemsByIdsAsync(List<Guid> inventoryIds) =>
        _context.Inventories
            .Where(i => inventoryIds.Contains(i.InventoryId))
            .Select(x => new InventoryItem
            {
                InventoryId = x.InventoryId,
                Price = x.Price
            })
            .ToListAsync();
}