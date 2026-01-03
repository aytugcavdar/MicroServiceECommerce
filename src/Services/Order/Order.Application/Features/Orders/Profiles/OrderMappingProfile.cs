using AutoMapper;
using BuildingBlocks.Messaging.IntegrationEvents;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Profiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<CreateOrderCommand, Domain.Entities.Order>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enums.OrderStatus.Submitted))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Domain.Entities.Order, OrderDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Address, AddressDto>();

        CreateMap<OrderItem, OrderItemMessage>();
    }
}
