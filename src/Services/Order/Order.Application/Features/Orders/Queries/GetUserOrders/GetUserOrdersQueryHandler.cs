using BuildingBlocks.Core.Paging;
using BuildingBlocks.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Services.Repositories;

namespace Order.Application.Features.Orders.Queries.GetUserOrders;

public class GetUserOrdersQueryHandler
    : IRequestHandler<GetUserOrdersQuery, Paginate<GetUserOrdersListItemDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetUserOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Paginate<GetUserOrdersListItemDto>> Handle(
        GetUserOrdersQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Base query: Kullanıcının siparişleri
        var query = _orderRepository.Query()
            .Include(o => o.OrderItems) // Item count için gerekli
            .Where(o => o.UserId == request.UserId);

        // 2. Filtreleme: Status
        if (request.StatusFilter.HasValue)
        {
            query = query.Where(o => o.Status == request.StatusFilter.Value);
        }

        // 3. Filtreleme: Tarih aralığı
        if (request.FromDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate <= request.ToDate.Value);
        }

        // 4. Sıralama: En yeni önce
        query = query.OrderByDescending(o => o.CreatedDate);

        // 5. Projection: Sadece gerekli alanları getir (performans için)
        var projectedQuery = query.Select(o => new GetUserOrdersListItemDto
        {
            OrderId = o.Id,
            Status = o.Status,
            StatusText = o.Status.ToString(),
            TotalPrice = o.TotalPrice,
            CreatedDate = o.CreatedDate,
            ItemCount = o.OrderItems.Count,
            City = o.Address.City
        });

        // 6. Sayfalama
        var paginatedResult = await projectedQuery.ToPaginateAsync(
            request.PageRequest.PageIndex,
            request.PageRequest.PageSize,
            cancellationToken
        );

        return paginatedResult;
    }
}