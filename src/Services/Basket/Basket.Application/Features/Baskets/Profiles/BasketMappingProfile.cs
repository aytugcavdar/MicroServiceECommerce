using AutoMapper;
using Basket.API.Entities;
using Basket.Application.Features.Baskets.Queries.GetBasket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Features.Baskets.Profiles;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<ShoppingCart, GetBasketQueryResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

        CreateMap<BasketItem, BasketItemDto>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
    }
}
