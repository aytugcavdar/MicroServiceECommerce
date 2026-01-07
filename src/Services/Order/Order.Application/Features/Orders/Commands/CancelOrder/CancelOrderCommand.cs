using MediatR;

namespace Order.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommand : IRequest<CancelOrderCommandResponse>
{
    public Guid OrderId { get; set; }
}
