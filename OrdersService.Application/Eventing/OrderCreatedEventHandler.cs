using System.Text.Json;
using Ardalis.GuardClauses;
using OrdersService.Core.Events;
using OrdersService.Core.Events.Models;
using OrdersService.Core.Metrics;
using OrdersService.Core.Models;
using OrdersService.Core.Persistence;

namespace OrdersService.Application.Eventing;

/// <summary>
/// Order Created Event Handler
/// </summary>
[EventHandler(OrderCreatedEvent.EventType)]
// ReSharper disable once UnusedType.Global
// Used by the Event Handler Registry
public class OrderCreatedEventHandler : IEventHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _itemRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="orderRepository"></param>
    /// <param name="itemRepository"></param>
    /// <param name="inventoryRepository"></param>
    /// <param name="logger"></param>
    public OrderCreatedEventHandler(IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IOrderItemRepository itemRepository,
        IInventoryRepository inventoryRepository,
        ILogger<OrderCreatedEventHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _itemRepository = itemRepository;
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles Order Created Event
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    public Task HandleAsync(string payload)
    {
        _logger.LogInformation("Handling Order Created Event");
        // I'm intentionally using the same model
        // as I don't want to create a new type for simplicity
        var eventData = JsonSerializer.Deserialize<OrderCreatedEvent>(payload);

        Guard.Against.Null(eventData);

        return ProcessOrderAsync(eventData);
    }

    private async Task ProcessOrderAsync(OrderCreatedEvent eventData)
    {
        using var scope = _logger.BeginScope(new List<KeyValuePair<string, object?>>
        {
            new("OrderId", eventData.OrderId)
        });

        try
        {
            _logger.LogInformation("Processing Order with Id {OrderId}", eventData.OrderId);

            await _unitOfWork.BeginTransactionAsync();
            var items = await _itemRepository.GetOrderItemsByOrderIdAsync(eventData.OrderId);
            var inventoryItems = (await _inventoryRepository
                    .GetInventoryItemsByIdsAsync(items.Select(x => x.InventoryItemId).ToList()))
                .ToDictionary(x => x.InventoryId, x => x);

            foreach (var item in items)
            {
                var inventoryItem = inventoryItems[item.InventoryItemId];
                item.SingleItemPrice = inventoryItem.Price;
                item.Subtotal = item.SingleItemPrice * item.Quantity;

                await _itemRepository.UpdateOrderItemAsync(item);
            }

            await _orderRepository.UpdateOrderStatusAsync(eventData.OrderId, OrderStatus.Processed);
            await _unitOfWork.CommitAsync();
            OrdersMetrics.OrdersProcessed.Add(1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Order {OrderId}", eventData.OrderId);
        }

        _logger.LogInformation("Order {OrderId} processed successfully", eventData.OrderId);
    }
}