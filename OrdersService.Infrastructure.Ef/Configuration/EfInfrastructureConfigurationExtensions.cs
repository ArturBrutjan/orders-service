using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Core.Events;
using OrdersService.Core.Persistence;
using OrdersService.Core.Persistence.Entities;
using OrdersService.Infrastructure.Ef.Eventing;
using OrdersService.Infrastructure.Ef.Persistence;

namespace OrdersService.Infrastructure.Ef.Configuration;

/// <summary>
/// Ef Infrastructure Config Extensions
/// </summary>
public static class EfInfrastructureConfigurationExtensions
{
    /// <summary>
    /// Add Ef Infrastructure
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddEfInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<OrdersServiceDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("OrdersServiceDb"),
                    builder => builder.MigrationsAssembly(typeof(OrdersServiceDbContext).Assembly.FullName));
                options.UseAsyncSeeding(async (context, _, ct) =>
                {
                    if (await context.Set<InventoryEntity>().AnyAsync(cancellationToken: ct))
                        return;
                    // Seeding some initial data for testing
                    context.Set<InventoryEntity>().AddRange(
                        new InventoryEntity { InventoryId = new Guid("00000000-0000-0000-0000-000000000001"), Price = 11 },
                        new InventoryEntity { InventoryId = new Guid("00000000-0000-0000-0000-000000000002"), Price = 12 },
                        new InventoryEntity { InventoryId = new Guid("00000000-0000-0000-0000-000000000003"), Price = 13 }
                    );

                    await context.SaveChangesAsync(ct);
                });
            })
            .AddScoped<IUnitOfWork, EfUnitOfWork>()
            .AddScoped<IEventBus, EfOutboxBus>()
            .AddScoped<IOrderRepository, OrderEfRepository>()
            .AddScoped<IOutboxRepository, EfOutboxRepository>()
            .AddScoped<IInventoryRepository, EfInventoryRepository>()
            .AddScoped<IOrderItemRepository, EfOrderItemRepository>();
}