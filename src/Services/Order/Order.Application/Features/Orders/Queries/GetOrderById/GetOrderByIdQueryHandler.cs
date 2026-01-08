using AutoMapper;
using BuildingBlocks.CrossCutting.Exceptions.types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Services.Repositories;
using Order.Domain.Enums;

namespace Order.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<GetOrderByIdResponse> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Siparişi OrderItems ile birlikte getir
        var order = await _orderRepository.Query()
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
            throw new NotFoundException("Order", request.OrderId);

        // 2. Authorization check (opsiyonel)
        if (request.RequestingUserId.HasValue &&
            order.UserId != request.RequestingUserId.Value)
        {
            throw new BusinessException("You don't have permission to view this order");
        }

        // 3. Response'a map et
        var response = _mapper.Map<GetOrderByIdResponse>(order);

        // 4. Ek bilgileri hesapla
        response.StatusText = order.Status.ToString();
        response.TotalItems = order.OrderItems.Count;

        // 5. Business logic: İptal edilebilir mi?
        response.CanBeCancelled = order.Status == OrderStatus.Submitted ||
                                  order.Status == OrderStatus.StockReserved;

        // 6. Business logic: Düzenlenebilir mi?
        response.CanBeEdited = order.Status == OrderStatus.Submitted;

        return response;
    }
}