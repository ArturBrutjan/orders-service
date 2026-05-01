namespace OrdersService.Core.Persistence;

/// <summary>
/// Unit of work interface
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Begins a transaction
    /// </summary>
    /// <returns></returns>
    Task BeginTransactionAsync();
    
    /// <summary>
    /// Commits the unit of work
    /// </summary>
    /// <returns></returns>
    Task CommitAsync();
}