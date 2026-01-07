namespace Order.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; }

    public DeleteOrderCommandResponse()
    {
        Message = "Order deleted successfully.";
    }
}