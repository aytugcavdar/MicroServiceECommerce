using MediatR;
using Order.Application.Features.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<CreateOrderCommandResponse>
{
    public Guid UserId { get; set; }
    public AddressDto Address { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }

    public CreateOrderCommand()
    {
        OrderItems = new List<CreateOrderItemDto>();
    }
}
