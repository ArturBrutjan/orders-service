using OrdersService.Core.Models;

namespace OrdersService.Core.Persistence;

/// <summary>
/// Repository for managing order items
/// </summary>
public interface IOrderItemRepository : IRepository
{
    /// <summary>
    /// Retrieves all order items associated with a given order ID
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId);
    
    /// <summary>
    /// Updates an existing order item
    /// </summary>
    /// <param name="orderItem"></param>
    /// <returns></returns>
    public Task UpdateOrderItemAsync(OrderItem orderItem);
}