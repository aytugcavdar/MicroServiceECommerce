using BuildingBlocks.CrossCutting.Exceptions.types;
using FluentValidation;
using Order.Application.Features.Orders.Commands;
using Order.Application.Features.Orders.Dtos;
using Order.Application.Services.Repositories;
using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Order.Application.Features.Orders.Rules;

public class OrderBusinessRules
{
    private readonly IOrderRepository _orderRepository;

    public OrderBusinessRules(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Domain.Entities.Order> OrderShouldExist(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetAsync(
            o => o.Id == orderId,
            cancellationToken: cancellationToken);

        if (order == null)
            throw new NotFoundException("Order", orderId);

        return order;
    }

    public void OrderShouldBelongToUser(Domain.Entities.Order order, Guid userId)
    {
        if (order.UserId != userId)
            throw new BusinessException("You don't have permission to access this order");
    }

    public void OrderShouldBeEditable(Domain.Entities.Order order)
    {
        if (order.Status != OrderStatus.Submitted)
        {
            throw new BusinessException(
                $"Order cannot be modified in {order.Status} status");
        }
    }

    public void OrderShouldBeCancellable(Domain.Entities.Order order)
    {
        var cancellableStatuses = new[]
        {
            OrderStatus.Submitted,
            OrderStatus.StockReserved
        };

        if (!cancellableStatuses.Contains(order.Status))
        {
            throw new BusinessException(
                $"Order cannot be canceled in {order.Status} status");
        }
    }

    public async Task UserShouldNotHaveTooManyPendingOrders(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var pendingCount = await _orderRepository.Query()
            .CountAsync(o => o.UserId == userId &&
                            o.Status != OrderStatus.Completed &&
                            o.Status != OrderStatus.Canceled &&
                            o.Status != OrderStatus.Failed,
                       cancellationToken);

        if (pendingCount >= 5)
        {
            throw new BusinessException(
                "You have too many pending orders. Please complete or cancel existing orders first.");
        }
    }

    public void OrderItemsShouldBeValid(List<CreateOrderItemDto> items)
    {
        if (!items.Any())
            throw new BusinessException("Order must contain at least one item");

        if (items.Count > 50)
            throw new BusinessException("Order cannot contain more than 50 items");

        var duplicateProducts = items
            .GroupBy(x => x.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateProducts.Any())
        {
            throw new BusinessException(
                $"Duplicate products found: {string.Join(", ", duplicateProducts)}");
        }
    }
}
