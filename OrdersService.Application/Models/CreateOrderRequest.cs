namespace OrdersService.Application.Models;

/// <summary>
/// Create Order Request
/// </summary>
/// <param name="CustomerId"></param>
/// <param name="Items"></param>
public record CreateOrderRequest(Guid CustomerId, List<OrderItemRequest> Items);