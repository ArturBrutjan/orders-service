namespace OrdersService.Core.Models;

/// <summary>
/// Order domain model
/// </summary>
public record Order
{
    /// <summary>
    /// Order Id
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Order Items
    /// </summary>
    public required List<OrderItem> OrderItems { get; set; }

    /// <summary>
    /// Order Status
    /// </summary>
    public required OrderStatus Status { get; set; }

    /// <summary>
    /// Customer Id
    /// </summary>
    public required Guid CustomerId { get; set; }

    /// <summary>
    /// Order Total
    /// </summary>
    public decimal? OrderTotal => OrderItems.Sum(item => item.Subtotal);
}