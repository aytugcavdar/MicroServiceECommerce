using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<GetOrderByIdResponse>
{
    public Guid OrderId { get; set; }

    public Guid? RequestingUserId { get; set; }
}
