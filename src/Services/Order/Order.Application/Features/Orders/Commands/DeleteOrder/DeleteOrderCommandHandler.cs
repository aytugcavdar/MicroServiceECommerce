using BuildingBlocks.CrossCutting.Exceptions;
using BuildingBlocks.CrossCutting.Exceptions.types;
using MediatR;
using Order.Application.Features.Orders.Dtos;
using Order.Application.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, DeleteOrderCommandResponse>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<DeleteOrderCommandResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(o => o.Id == request.OrderId);

        if (order == null)
            throw new EntityNotFoundException("Order", request.OrderId);

        await _orderRepository.DeleteAsync(order); 
        return new DeleteOrderCommandResponse { Id = order.Id };
    }
}
