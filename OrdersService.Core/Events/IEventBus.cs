using OrdersService.Core.Events.Models;

namespace OrdersService.Core.Events;

/// <summary>
/// Publisher for domain events
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publish Domain Event
    /// </summary>
    /// <param name="evt"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent evt) where TEvent : IDomainEvent;
}