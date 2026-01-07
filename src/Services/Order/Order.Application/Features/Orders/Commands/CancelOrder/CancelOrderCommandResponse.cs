namespace Order.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public CancelOrderCommandResponse()
    {
        IsSuccess = true;
        Message = "Order canceled successfully.";
    }
}