using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage;
using OrdersService.Core.Persistence;

namespace OrdersService.Infrastructure.Ef.Persistence;

/// <summary>
/// Ef Unit Of Work
/// </summary>
public sealed class EfUnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private readonly OrdersServiceDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="context"></param>
    public EfUnitOfWork(OrdersServiceDbContext context) => _context = context;

    /// <inheritdoc />
    [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning",
        Justification = "You cannot begin a transaction twice")]
    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started");
        
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc />
    public async Task CommitAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction not started");
        
        await _transaction.CommitAsync();
        await _context.SaveChangesAsync();
        await DisposeAsync();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        _transaction?.Dispose();
        _disposed = true;
    }
    
    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_transaction != null)
            await _transaction.DisposeAsync();
        _disposed = true;
    }
}