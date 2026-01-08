using AutoMapper;
using BuildingBlocks.Messaging.IntegrationEvents;
using Order.Application.Features.Orders.Commands.CreateOrder;
using Order.Application.Features.Orders.Dtos;
using Order.Application.Features.Orders.Queries.GetOrderById;
using Order.Application.Features.Orders.Queries.GetUserOrders;
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

        CreateMap<Domain.Entities.Order, GetOrderByIdResponse>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.OrderItems.Count))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<Domain.Entities.Order, GetUserOrdersListItemDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StatusText, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.OrderItems.Count))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

        CreateMap<Domain.Entities.Order, OrderDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<OrderItem, OrderItemDto>();

        CreateMap<Address, AddressDto>();
        CreateMap<OrderItem, OrderItemMessage>();
    }
}
