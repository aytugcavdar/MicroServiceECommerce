using MediatR;
using Order.Application.Services.Repositories;
using Order.Domain.Entities;

namespace Order.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderCommandResponse>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<UpdateOrderCommandResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetAsync(o => o.Id == request.OrderId);

        if (order == null)
            throw new Exception("Sipariş bulunamadı.");
        var newAddress = new Address(
            street: request.Address.Street,
            city: request.Address.City,
            state: request.Address.State,
            country: request.Address.Country,
            zipCode: request.Address.ZipCode
        );

        order.UpdateAddress(newAddress);

        await _orderRepository.UpdateAsync(order);

        return new UpdateOrderCommandResponse
        {
            OrderId = order.Id,
            UpdatedAddress = request.Address,
            Message = "Sipariş adresi başarıyla güncellendi."
        };
    }
}