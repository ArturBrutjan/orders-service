using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence.EntityConfigs;

/// <summary>
/// Configuration for the Order entity in the database.
/// </summary>
public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    /// <summary>
    /// Configures the entity type for OrderEntity.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(x => x.OrderId);
        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.HasIndex(x => x.OrderId).IsUnique();
    }
}