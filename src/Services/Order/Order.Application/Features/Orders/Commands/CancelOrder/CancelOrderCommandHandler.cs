using MediatR;
using Order.Application.Services.Repositories;
using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, CancelOrderCommandResponse>
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CancelOrderCommandResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(o => o.Id == request.OrderId);
        if (order == null) throw new Exception("Order not found"); 


        order.UpdateStatus(OrderStatus.Canceled);
        await _orderRepository.UpdateAsync(order);

        return new CancelOrderCommandResponse { IsSuccess = true };
    }
}
