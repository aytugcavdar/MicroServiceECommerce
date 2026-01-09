namespace Order.Application.Features.Orders.Queries.GetOrderStatistics;

public class OrderStatisticsResponse
{
    public int TotalOrders { get; set; }

    public int CompletedOrders { get; set; }

    public int CancelledOrders { get; set; }

    public int PendingOrders { get; set; }

    public int FailedOrders { get; set; }

    public decimal TotalSpending { get; set; }

    public decimal AverageOrderValue { get; set; }

    public decimal HighestOrderValue { get; set; }

    public DateTime? LastOrderDate { get; set; }

    public DateTime? FirstOrderDate { get; set; }
}