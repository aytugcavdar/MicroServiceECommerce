using AutoMapper;
using Payment.Application.Features.Payments.Dtos;

namespace Payment.Application.Features.Payments.Profiles;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<Domain.Entities.Payment, PaymentDto>();
    }
}
