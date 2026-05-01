namespace OrdersService.Core.Events;

/// <summary>
/// Event Handler definition
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EventHandlerAttribute : Attribute
{
    /// <summary>
    /// Event Type associated with the handler
    /// </summary>
    public string EventType { get; }
    
    /// <summary>
    /// Initializes a new instance of the EventHandlerAttribute class
    /// </summary>
    /// <param name="eventType"></param>
    public EventHandlerAttribute(string eventType) => EventType = eventType;
}