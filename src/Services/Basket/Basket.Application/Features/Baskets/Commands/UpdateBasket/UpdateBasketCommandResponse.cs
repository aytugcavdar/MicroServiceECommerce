namespace Basket.Application.Features.Baskets.Commands.UpdateBasket;

public class UpdateBasketCommandResponse
{
    public string UserName { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public int ItemCount { get; set; }
}
