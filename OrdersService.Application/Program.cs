using Microsoft.EntityFrameworkCore;
using OrdersService.Application.Configuration;
using OrdersService.Application.Endpoints;
using OrdersService.Infrastructure.Ef.Configuration;
using OrdersService.Infrastructure.Ef.Persistence;
using OrdersService.Infrastructure.Rabbit.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder
    .Services
    .AddOpenApi()
    .AddValidation()
    .AddProblemDetails()
    .AddEventingConfiguration(builder.Configuration)
    .AddRabbitMqInfrastructure(builder.Configuration)
    .AddEfInfrastructure(builder.Configuration)
    .AddOrderServiceTelemetry();

var app = builder.Build();

app.MapOpenApi();
app.UseExceptionHandler(); 
app.UseStatusCodePages(); 
app.MapPrometheusScrapingEndpoint();
app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });

app.MapOrderEndpoints();

// Dirty but simple migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersServiceDbContext>();
    await db.Database.MigrateAsync();
}

await app.RunAsync();