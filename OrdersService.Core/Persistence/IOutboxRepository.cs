using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Core.Persistence;

/// <summary>
/// Outbox Repository
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    /// Gets a batch of messages from Outbox Table.
    /// </summary>
    /// <param name="batchSize"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<List<OutboxMessage>> GetOutboxMessageBatchAsync(int batchSize, CancellationToken ct = default);
    
    /// <summary>
    /// Updates Outbox Message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task UpdateOutboxMessageAsync(OutboxMessage message);
}