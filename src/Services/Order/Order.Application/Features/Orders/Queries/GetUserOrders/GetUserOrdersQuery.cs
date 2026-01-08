using BuildingBlocks.Core.Paging;
using BuildingBlocks.Core.Requests;
using MediatR;
using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Queries.GetUserOrders;

public class GetUserOrdersQuery : IRequest<Paginate<GetUserOrdersListItemDto>>
{
    public Guid UserId { get; set; }
    public PageRequest PageRequest { get; set; } = new();

    public OrderStatus? StatusFilter { get; set; }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
