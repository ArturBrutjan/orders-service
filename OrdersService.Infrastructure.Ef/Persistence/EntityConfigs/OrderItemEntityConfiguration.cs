using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence.EntityConfigs;

/// <summary>
/// Configuration for the Order Item entity in the database.
/// </summary>
public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    /// <summary>
    /// Configures the entity type for OrderItemEntity.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(x => x.OrderItemId);
        builder.Property(x => x.OrderItemId).IsRequired();
        builder.Property(x => x.OrderItemId).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.OrderId).IsRequired();

        builder.HasIndex(x => x.OrderItemId);
        builder.HasIndex(x => x.OrderId);
        builder.HasIndex(x => x.InventoryItemId);

        builder.HasIndex(x => new { x.OrderId, x.InventoryItemId }).IsUnique();

        builder
            .HasOne<OrderEntity>()
            .WithMany()
            .HasForeignKey(x => x.OrderId);

        builder
            .HasOne<InventoryEntity>()
            .WithMany()
            .HasForeignKey(x => x.InventoryItemId);
    }
}