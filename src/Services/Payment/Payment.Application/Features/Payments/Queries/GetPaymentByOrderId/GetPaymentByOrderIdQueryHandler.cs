using AutoMapper;
using MediatR;
using Payment.Application.Features.Payments.Dtos;
using Payment.Application.Services;

namespace Payment.Application.Features.Payments.Queries.GetPaymentByOrderId;

public class GetPaymentByOrderIdQueryHandler : IRequestHandler<GetPaymentByOrderIdQuery, GetPaymentByOrderIdResponse>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public GetPaymentByOrderIdQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<GetPaymentByOrderIdResponse> Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);

        if (payment == null)
        {
            return new GetPaymentByOrderIdResponse
            {
                Found = false,
                Payment = null
            };
        }

        return new GetPaymentByOrderIdResponse
        {
            Found = true,
            Payment = _mapper.Map<PaymentDto>(payment)
        };
    }
}
