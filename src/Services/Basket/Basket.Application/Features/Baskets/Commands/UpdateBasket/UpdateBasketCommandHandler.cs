using Basket.API.Entities;
using Basket.Application.Services;
using MediatR;

namespace Basket.Application.Features.Baskets.Commands.UpdateBasket;

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, UpdateBasketCommandResponse>
{
    private readonly IBasketRepository _repository;

    public UpdateBasketCommandHandler(IBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateBasketCommandResponse> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = new ShoppingCart(request.UserName);

        foreach (var item in request.Items)
        {
            basket.AddItem(new BasketItem(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.Color,
                item.Price
            ));
        }

        var updatedBasket = await _repository.UpdateBasketAsync(basket, cancellationToken);

        return new UpdateBasketCommandResponse
        {
            UserName = updatedBasket.UserName,
            TotalPrice = updatedBasket.TotalPrice,
            ItemCount = updatedBasket.Items.Count
        };
    }
}