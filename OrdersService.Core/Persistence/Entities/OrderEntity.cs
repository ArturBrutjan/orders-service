using OrdersService.Core.Models;

namespace OrdersService.Core.Persistence.Entities;

/// <summary>
/// Order Entity
/// </summary>
public class OrderEntity
{
    /// <summary>
    /// Order Id
    /// </summary>
    public required Guid OrderId { get; init; }

    /// <summary>
    /// Order Status
    /// </summary>
    public required OrderStatus Status { get; set; }

    /// <summary>
    /// Customer Id
    /// </summary>
    public required Guid CustomerId { get; set; }
}