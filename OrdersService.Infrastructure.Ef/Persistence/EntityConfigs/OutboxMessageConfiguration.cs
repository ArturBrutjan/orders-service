using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Core.Persistence.Entities;

namespace OrdersService.Infrastructure.Ef.Persistence.EntityConfigs;

/// <summary>
/// Configures the entity type for OutboxMessage.
/// </summary>
public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    /// <summary>
    /// Configures the entity type for OutboxMessage.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.MessageId);
        builder.Property(x => x.MessageId).IsRequired();
        builder.HasIndex(x => x.MessageId).IsUnique();
        builder.Property(x => x.Topic).IsRequired();
        builder.Property(x => x.Key).IsRequired();
        builder.Property(x => x.Payload).IsRequired();
    }
}