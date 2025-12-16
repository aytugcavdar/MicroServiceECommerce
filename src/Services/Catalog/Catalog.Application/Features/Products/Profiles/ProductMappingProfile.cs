using AutoMapper;
using BuildingBlocks.Core.Paging;
using Catalog.Application.Features.Products.Commands.Create;
using Catalog.Application.Features.Products.Commands.Delete;
using Catalog.Application.Features.Products.Commands.Update;
using Catalog.Application.Features.Products.Queries.GetListProduct;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Profiles;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Query Mappings
        CreateMap<Product, GetListProductListItemDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<Paginate<Product>, Paginate<GetListProductListItemDto>>();

        // Command Mappings
        CreateMap<Product, CreateProductCommandResponse>()
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate));

        CreateMap<Product, UpdateProductCommandResponse>();

        CreateMap<Product, DeleteProductCommandResponse>();
    }
}
