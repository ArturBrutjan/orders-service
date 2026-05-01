using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OrdersService.Core.Configuration;
using OrdersService.Core.Events;
using OrdersService.Core.Events.Models;
using OrdersService.Core.Persistence.Entities;
using OrdersService.Infrastructure.Ef.Persistence;

namespace OrdersService.Infrastructure.Ef.Eventing;

/// <summary>
/// Ef Outbox Publisher
/// </summary>
public class EfOutboxBus : IEventBus
{
    private readonly OrdersServiceDbContext _context;
    private readonly string _queueName;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="queueSettngs"></param>
    public EfOutboxBus(OrdersServiceDbContext context, IOptions<QueueSettings> queueSettngs)
    {
        _context = context;
        _queueName = queueSettngs.Value.QueueName;
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent evt) where TEvent : IDomainEvent
    {
        // Serialize the object to JSON bytes
        var payload = JsonSerializer.SerializeToUtf8Bytes(evt, evt.GetType());

        // Convert the PartitionKey string to bytes
        var key = Encoding.UTF8.GetBytes(evt.GetPartitionKey());

        var message = OutboxMessage.CreateNew(
            payload,
            key,
            _queueName,
            evt.Type);

        await _context.OutboxMessages.AddAsync(message);
    }
}