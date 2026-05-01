using Microsoft.EntityFrameworkCore;
using OrdersService.Core.Models;
using OrdersService.Core.Persistence;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence;

/// <summary>
/// Order Ef Repository
/// </summary>
public class OrderEfRepository : IOrderRepository
{
    private readonly OrdersServiceDbContext _dbContext;
    
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="dbContext"></param>
    public OrderEfRepository(OrdersServiceDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public async Task<Guid> CreateOrderAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(new OrderEntity
        {
            OrderId = order.Id,
            Status = order.Status
        });

        await _dbContext.OrderItems.AddRangeAsync(order.OrderItems.Select(x => new OrderItemEntity
        {
            OrderItemId = x.Id,
            OrderId = order.Id,
            InventoryItemId = x.InventoryItemId,
            Quantity = x.Quantity
        }));

        return order.Id;
    }

    /// <inheritdoc />
    public Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status) =>
        _dbContext.Orders
            .Where(x => x.OrderId == orderId)
            .ExecuteUpdateAsync(p =>
                p.SetProperty(x => x.Status, status));
}