using AutoMapper;
using BuildingBlocks.Messaging.IntegrationEvents;
using MassTransit;
using MediatR;
using Order.Application.Services.Repositories;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(
            request.Address.Street,
            request.Address.City,
            request.Address.State,
            request.Address.Country,
            request.Address.ZipCode);

        var order = new Domain.Entities.Order(request.UserId, address);

        foreach (var item in request.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.Price, item.Quantity);
        }

        await _orderRepository.AddAsync(order);

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            BuyerId = order.UserId,
            OrderItems = order.OrderItems.Select(x => new OrderItemMessage
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList()
        };

        await _publishEndpoint.Publish(orderCreatedEvent, cancellationToken);

        return new CreateOrderCommandResponse
        {
            OrderId = order.Id,
            Status = order.Status.ToString(),
            Message = "Sipariş alındı, stok kontrol süreci başlatıldı."
        };
    }
}

