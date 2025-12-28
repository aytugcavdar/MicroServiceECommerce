using AutoMapper;
using Basket.Application.Services;
using MediatR;

namespace Basket.Application.Features.Baskets.Queries.GetBasket;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, GetBasketQueryResponse>
{
    private readonly IBasketRepository _repository;
    private readonly IMapper _mapper;

    public GetBasketQueryHandler(IBasketRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetBasketQueryResponse> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _repository.GetBasketAsync(request.UserName, cancellationToken);

        if (basket == null)
        {
            return new GetBasketQueryResponse
            {
                UserName = request.UserName,
                Items = new List<BasketItemDto>(),
                TotalPrice = 0
            };
        }

        return _mapper.Map<GetBasketQueryResponse>(basket);
    }
}
