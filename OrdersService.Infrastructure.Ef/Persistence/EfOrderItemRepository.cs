using Microsoft.EntityFrameworkCore;
using OrdersService.Core.Models;
using OrdersService.Core.Persistence;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence;

/// <summary>
/// Implementation of the IOrderItemRepository interface using Entity Framework
/// </summary>
public class EfOrderItemRepository : IOrderItemRepository
{
    private readonly OrdersServiceDbContext _context;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="context"></param>
    public EfOrderItemRepository(OrdersServiceDbContext context) => _context = context;

    /// <inheritdoc />
    public Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId) =>
        _context.OrderItems.Where(item => item.OrderId == orderId).Select(x => new OrderItem
        {
            Id = x.OrderItemId,
            OrderId = x.OrderId,
            InventoryItemId = x.InventoryItemId,
            Quantity = x.Quantity,
            SingleItemPrice = x.SingleItemPrice,
            Subtotal = x.Subtotal
        }).ToListAsync();

    /// <inheritdoc />
    public Task UpdateOrderItemAsync(OrderItem orderItem)
    {
        _context.Update(new OrderItemEntity
        {
            OrderItemId = orderItem.Id,
            OrderId = orderItem.OrderId,
            InventoryItemId = orderItem.InventoryItemId,
            Quantity = orderItem.Quantity,
            SingleItemPrice = orderItem.SingleItemPrice,
            Subtotal = orderItem.Subtotal
        });

        return Task.CompletedTask;
    }
}