namespace Basket.Application.Features.Baskets.Commands.DeleteBasket;

public class DeleteBasketCommandResponse
{
    public bool IsDeleted { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime DeletedAt { get; set; }
}
