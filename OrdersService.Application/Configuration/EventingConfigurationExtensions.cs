using OrdersService.Core.Configuration;
using OrdersService.Core.Events;
using OrdersService.Core.Helpers;

namespace OrdersService.Application.Configuration;

/// <summary>
/// Extension methods for configuring eventing-related settings.
/// </summary>
public static class EventingConfigurationExtensions
{
    /// <summary>
    /// Configures eventing-related services and settings.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventingConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var registry = new EventHandlerRegistry();
        registry.RegisterHandlersFromAssembly(typeof(Program).Assembly);
        services.AddSingleton(registry);

        foreach (var handler in GetHandlerTypes())
        {
            services.AddScoped(handler);
        }

        services.Configure<QueueSettings>(configuration.GetSection(QueueSettings.SectionName));
        
        return services;
    }

    private static IEnumerable<Type> GetHandlerTypes() =>
        EventHandlerTypeFinder.GetHandlerTypes(typeof(Program).Assembly);
}