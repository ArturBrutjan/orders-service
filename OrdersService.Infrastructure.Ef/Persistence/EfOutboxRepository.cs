using Microsoft.EntityFrameworkCore;
using OrdersService.Core.Persistence;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence;

/// <summary>
/// Ef Outbox Repository
/// </summary>
public class EfOutboxRepository : IOutboxRepository
{
    private readonly OrdersServiceDbContext _context;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="context"></param>
    public EfOutboxRepository(OrdersServiceDbContext context) => _context = context;

    /// <inheritdoc />
    public Task<List<OutboxMessage>> GetOutboxMessageBatchAsync(int batchSize, CancellationToken ct = default) =>
        _context.Set<OutboxMessage>()
            .FromSql($$"""
                       SELECT * FROM
                       "OutboxMessages" 
                       WHERE "State" = 0 
                       ORDER BY "MessageId" 
                       LIMIT {{batchSize}} 
                       FOR UPDATE SKIP LOCKED 
                       """)
            .ToListAsync(ct);

    /// <inheritdoc />
    public Task UpdateOutboxMessageAsync(OutboxMessage message)
    {
        _context.OutboxMessages.Update(message);
        return Task.CompletedTask;
    }
}