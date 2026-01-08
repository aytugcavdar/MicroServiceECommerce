using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Services.Repositories;
using Order.Domain.Enums;

namespace Order.Application.Features.Orders.Queries.GetOrderStatistics;

public class GetOrderStatisticsQueryHandler: IRequestHandler<GetOrderStatisticsQuery, GetOrderStatisticsResponse>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderStatisticsQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<GetOrderStatisticsResponse> Handle(
        GetOrderStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var userOrders = await _orderRepository.Query()
            .Where(o => o.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (!userOrders.Any())
        {
            return new GetOrderStatisticsResponse();
        }
        var completedOrders = userOrders
            .Where(o => o.Status == OrderStatus.Completed)
            .ToList();

        var response = new GetOrderStatisticsResponse
        {
            TotalOrders = userOrders.Count,

            CompletedOrders = completedOrders.Count,

            CancelledOrders = userOrders.Count(o => o.Status == OrderStatus.Canceled),

            PendingOrders = userOrders.Count(o =>
                o.Status == OrderStatus.Submitted ||
                o.Status == OrderStatus.StockReserved ||
                o.Status == OrderStatus.PaymentFailed),

            FailedOrders = userOrders.Count(o => o.Status == OrderStatus.Failed),

            TotalSpending = completedOrders.Sum(o => o.TotalPrice),

            AverageOrderValue = completedOrders.Any()
                ? completedOrders.Average(o => o.TotalPrice)
                : 0,

            HighestOrderValue = completedOrders.Any()
                ? completedOrders.Max(o => o.TotalPrice)
                : 0,
            LastOrderDate = userOrders.Max(o => o.CreatedDate),
            FirstOrderDate = userOrders.Min(o => o.CreatedDate)
        };

        return response;
    }
}