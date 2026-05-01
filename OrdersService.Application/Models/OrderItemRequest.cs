using System.ComponentModel.DataAnnotations;

namespace OrdersService.Application.Models;

/// <summary>
/// Order Item Request
/// </summary>
/// <param name="ProductId"></param>
/// <param name="Quantity"></param>
public record OrderItemRequest(Guid ProductId, [Range(1, int.MaxValue)]int Quantity);