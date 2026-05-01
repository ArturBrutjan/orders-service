using System.Reflection;
using OrdersService.Core.Helpers;

namespace OrdersService.Core.Events;

/// <summary>
/// Event Handler Registry
/// </summary>
public class EventHandlerRegistry
{
    private readonly Dictionary<string, Type> _handlers = new();

    /// <summary>
    /// Register Handlers from assembly
    /// </summary>
    /// <param name="assembly"></param>
    public void RegisterHandlersFromAssembly(Assembly assembly)
    {
        var types = EventHandlerTypeFinder.GetHandlerTypes(assembly);

        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<EventHandlerAttribute>()!;
            _handlers[attribute.EventType] = type;
        }
    }

    /// <summary>
    /// Get Handler Type by Event Type
    /// </summary>
    /// <param name="eventType"></param>
    /// <returns></returns>
    public Type? GetHandlerType(string eventType) =>
        _handlers.GetValueOrDefault(eventType);
}