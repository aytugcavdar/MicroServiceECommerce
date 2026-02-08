using MediatR;
using Payment.Application.Features.Payments.Dtos;

namespace Payment.Application.Features.Payments.Queries.GetPaymentByOrderId;

public class GetPaymentByOrderIdQuery : IRequest<GetPaymentByOrderIdResponse>
{
    public Guid OrderId { get; set; }
}

public class GetPaymentByOrderIdResponse
{
    public PaymentDto? Payment { get; set; }
    public bool Found { get; set; }
}
