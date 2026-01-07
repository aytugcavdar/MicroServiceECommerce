using MediatR;
using Order.Application.Features.Orders.Dtos;

namespace Order.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest<UpdateOrderCommandResponse>
{
    public Guid OrderId { get; set; }
    public AddressDto Address { get; set; }
}
