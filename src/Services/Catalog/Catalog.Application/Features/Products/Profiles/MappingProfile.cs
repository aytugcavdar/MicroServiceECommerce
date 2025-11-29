using AutoMapper;
using BuildingBlocks.Core.Paging;
using Catalog.Application.Features.Products.Queries.GetListProduct;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Profiles;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<Product, GetListProductListItemDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<Paginate<Product>, Paginate<GetListProductListItemDto>>();
    }
}
