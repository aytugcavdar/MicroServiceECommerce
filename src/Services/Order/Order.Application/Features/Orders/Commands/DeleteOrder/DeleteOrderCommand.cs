using MediatR;

namespace Order.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommand : IRequest<DeleteOrderCommandResponse>
{
    public Guid OrderId { get; set; }
}