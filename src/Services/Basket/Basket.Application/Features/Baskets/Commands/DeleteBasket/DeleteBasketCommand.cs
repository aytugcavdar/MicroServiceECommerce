using MediatR;

namespace Basket.Application.Features.Baskets.Commands.DeleteBasket;

public class DeleteBasketCommand : IRequest<DeleteBasketCommandResponse>
{
    public string UserName { get; set; } = string.Empty;
}
