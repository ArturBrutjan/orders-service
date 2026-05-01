namespace OrdersService.Core.Events;

/// <summary>
/// Event Handler interface for handling events
/// </summary>
public interface IEventHandler
{
    /// <summary>
    /// Handles event
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    Task HandleAsync(string payload);
}