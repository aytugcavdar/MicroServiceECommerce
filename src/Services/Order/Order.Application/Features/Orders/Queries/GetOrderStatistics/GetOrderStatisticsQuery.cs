using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Queries.GetOrderStatistics;

public class GetOrderStatisticsQuery : IRequest<GetOrderStatisticsResponse>
{
    public Guid UserId { get; set; }
}