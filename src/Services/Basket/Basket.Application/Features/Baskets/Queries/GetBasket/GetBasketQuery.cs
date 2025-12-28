using Basket.API.Entities;
using MediatR;

namespace Basket.Application.Features.Baskets.Queries.GetBasket;

public class GetBasketQuery : IRequest<GetBasketQueryResponse>
{
    public string UserName { get; set; } = string.Empty;
}

public class GetBasketQueryResponse
{
    public string UserName { get; set; } = string.Empty;
    public List<BasketItemDto> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}

public class BasketItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Color { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal TotalPrice { get; set; }
}
