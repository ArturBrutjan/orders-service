using OpenTelemetry.Metrics;
using OrdersService.Core.Metrics;

namespace OrdersService.Application.Configuration;

/// <summary>
/// Telemetry configuration extensions for the Order Service.
/// </summary>
public static class TelemetryConfigurationExtensions
{
    /// <summary>
    /// Configures OpenTelemetry metrics for the Order Service.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOrderServiceTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter()
                .AddMeter(OrdersMetrics.OrdersMeter.Name)
                .AddPrometheusExporter());
        return services;
    }
}