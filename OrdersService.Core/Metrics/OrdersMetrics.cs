using System.Diagnostics.Metrics;

namespace OrdersService.Core.Metrics;

/// <summary>
/// Metrics related to the OrdersService.
/// </summary>
public static class OrdersMetrics
{
    /// <summary>
    /// Meter instance for tracking OrdersService metrics.
    /// </summary>
    public static readonly Meter OrdersMeter = new("OrdersService.Metrics", "1.0.0");

    /// <summary>
    /// Counter for tracking the number of orders processed by the service.
    /// </summary>
    public static readonly Counter<long> OrdersProcessed = OrdersMeter.CreateCounter<long>("orders.processed");
}