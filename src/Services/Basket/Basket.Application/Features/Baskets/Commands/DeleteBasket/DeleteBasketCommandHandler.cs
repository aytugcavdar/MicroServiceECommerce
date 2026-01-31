using Basket.Application.Services;
using MediatR;

namespace Basket.Application.Features.Baskets.Commands.DeleteBasket;

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, DeleteBasketCommandResponse>
{
    private readonly IBasketRepository _repository;

    public DeleteBasketCommandHandler(IBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteBasketCommandResponse> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var existingBasket = await _repository.GetBasketAsync(request.UserName, cancellationToken);

        if (existingBasket == null)
        {
            throw new KeyNotFoundException($"'{request.UserName}' için sepet bulunamadı.");
        }

        var isDeleted = await _repository.DeleteBasketAsync(request.UserName, cancellationToken);

        return new DeleteBasketCommandResponse
        {
            IsDeleted = isDeleted,
            UserName = request.UserName,
            DeletedAt = DateTime.UtcNow
        };
    }
}
