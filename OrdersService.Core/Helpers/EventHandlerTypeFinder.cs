using System.Reflection;
using OrdersService.Core.Events;

namespace OrdersService.Core.Helpers;

/// <summary>
/// Helper class for finding event handler types.
/// </summary>
public static class EventHandlerTypeFinder
{
    /// <summary>
    /// Finds event handler types within the specified assembly.
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetHandlerTypes(Assembly assembly) =>
        assembly.GetTypes()
            .Where(t => t.GetCustomAttributes<EventHandlerAttribute>().Any()
                        && typeof(IEventHandler).IsAssignableFrom(t));
}