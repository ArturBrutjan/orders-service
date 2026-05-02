using Microsoft.AspNetCore.Mvc;
using OrdersService.Application.Models;
using OrdersService.Core.Events;
using OrdersService.Core.Events.Models;
using OrdersService.Core.Models;
using OrdersService.Core.Persistence;

namespace OrdersService.Application.Endpoints;

/// <summary>
/// Order Endpoints
/// </summary>
public static class OrderEndpoints
{
    /// <summary>
    /// Map Order Endpoints
    /// </summary>
    /// <param name="app"></param>
    public static void MapOrderEndpoints(this WebApplication app) =>
        app.MapPost("/orders", CreateOrderAsync)
            .Produces(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithDescription("Create a new order");

    private static async Task<IResult> CreateOrderAsync(
        [FromBody] CreateOrderRequest request,
        [FromServices] IUnitOfWork unitOfWork,
        [FromServices] IOrderRepository orderRepo,
        [FromServices] IInventoryRepository inventoryRepo,
        [FromServices] IEventBus eventBus)
    {
        
        var (isValid, missingInventory) = await ValidateInventory();
        if (!isValid)
        {
            var errors = new Dictionary<string, string[]>();
            for (var i = 0; i < request.Items.Count; i++)
            {
                var item = request.Items[i];
                if (missingInventory.Contains(item.ProductId))
                {
                    errors.Add($"Items[{i}].ProductId", [$"Inventory missing for item {item.ProductId}."]);
                }
            }

            return Results.ValidationProblem(errors);
        }

        var order = CreateOrder();
        var orderCreatedEvent = CreateEvent();

        await unitOfWork.BeginTransactionAsync();

        await orderRepo.CreateOrderAsync(order);
        await eventBus.PublishAsync(orderCreatedEvent);

        await unitOfWork.CommitAsync();

        return Results.Created();

        async Task<(bool IsValid, Guid[] missingInventory)> ValidateInventory()
        {
            var itemIds = request.Items.Select(x => x.ProductId).ToList();
            var existingItems = await inventoryRepo.GetInventoryItemsByIdsAsync(itemIds);
            return existingItems.Count == itemIds.Count 
                ? (true, []) 
                : (false, itemIds.Except(existingItems.Select(x => x.InventoryId)).ToArray());
        }

        Order CreateOrder()
        {
            var orderId = Guid.NewGuid();
            return new Order
            {
                Id = orderId,
                Status = OrderStatus.New,
                CustomerId = request.CustomerId,
                OrderItems = request.Items.Select(x => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    InventoryItemId = x.ProductId,
                    OrderId = orderId,
                    Quantity = x.Quantity
                }).ToList()
            };
        }

        OrderCreatedEvent CreateEvent()
        {
            return new OrderCreatedEvent
            {
                OrderId = order.Id,
                Items = order.OrderItems.Select(x => new OrderItemsEventPayload
                {
                    OrderItemId = x.Id,
                    Amount = x.Quantity
                }).ToList()
            };
        }
    }
}