using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands;

public class CreateOrderCommandResponse
{
    public Guid OrderId { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
}
