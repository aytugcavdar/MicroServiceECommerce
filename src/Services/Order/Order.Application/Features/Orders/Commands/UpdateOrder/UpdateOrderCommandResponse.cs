using Order.Application.Features.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandResponse
{
    public Guid OrderId { get; set; }
    public AddressDto UpdatedAddress { get; set; }
    public string Message { get; set; }

    public UpdateOrderCommandResponse()
    {
        Message = "Order updated successfully.";
    }
}
