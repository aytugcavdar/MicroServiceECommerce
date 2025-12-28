namespace Basket.Application.Features.Baskets.Commands.Checkout;

public class CheckoutBasketCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
