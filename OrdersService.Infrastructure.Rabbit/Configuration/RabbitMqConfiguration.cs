using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Infrastructure.Rabbit.Connection;
using OrdersService.Infrastructure.Rabbit.Workers;

namespace OrdersService.Infrastructure.Rabbit.Configuration;

/// <summary>
/// Configuration for RabbitMQ integration within the application.
/// </summary>
public static class RabbitMqConfiguration
{
    /// <summary>
    /// Add Event Handling
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddRabbitMqInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMqConnectionBuilder, RabbitMqConnectionBuilder>();

        services.AddOptionsWithValidateOnStart<RabbitMqOptions>()
            .Bind(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddHostedService<RabbitMqConsumerService>();
        services.AddHostedService<RabbitMqProducerService>();


        return services;
    }
}