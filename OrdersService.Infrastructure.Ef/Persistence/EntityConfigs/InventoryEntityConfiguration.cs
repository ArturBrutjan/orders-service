using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence.EntityConfigs;

/// <summary>
/// Configures the entity type for InventoryEntity.
/// </summary>
public class InventoryEntityConfiguration : IEntityTypeConfiguration<InventoryEntity>
{
    /// <summary>
    /// Configures the entity type for InventoryEntity.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<InventoryEntity> builder)
    {
        builder.HasKey(x => x.InventoryId);
        builder.Property(x => x.InventoryId).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.HasIndex(x => x.InventoryId).IsUnique();
    }
}