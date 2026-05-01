using Microsoft.EntityFrameworkCore;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence;

/// <summary>
/// Orders Service DbContext
/// </summary>
public class OrdersServiceDbContext : DbContext
{
    /// <summary>
    /// Orders set
    /// </summary>
    public DbSet<OrderEntity> Orders { get; set; }

    /// <summary>
    /// Outbox Messages set
    /// </summary>
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    /// <summary>
    /// Inventories set
    /// </summary>
    public DbSet<InventoryEntity> Inventories { get; set; }

    /// <summary>
    /// Order items set
    /// </summary>
    public DbSet<OrderItemEntity> OrderItems { get; set; }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="options"></param>
    public OrdersServiceDbContext(DbContextOptions<OrdersServiceDbContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersServiceDbContext).Assembly);

}