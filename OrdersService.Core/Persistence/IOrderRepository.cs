using OrdersService.Core.Models;

namespace OrdersService.Core.Persistence;

/// <summary>
/// Order Repository
/// </summary>
public interface IOrderRepository : IRepository
{
    /// <summary>
    /// Creates an order
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<Guid> CreateOrderAsync(Order order);
    
    /// <summary>
    /// Updates the order status
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
}