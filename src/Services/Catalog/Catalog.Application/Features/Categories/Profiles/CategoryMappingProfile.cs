using AutoMapper;
using BuildingBlocks.Core.Paging;
using Catalog.Application.Features.Categories.Commands.Create;
using Catalog.Application.Features.Categories.Commands.Delete;
using Catalog.Application.Features.Categories.Commands.Uptade;
using Catalog.Application.Features.Categories.Queries.GetList;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Profiles;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, GetListCategoryListItemDto>()
            .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));

        CreateMap<Paginate<Category>, Paginate<GetListCategoryListItemDto>>();

        // Create Komutu İçin Mapping
        CreateMap<Category, CreateCategoryCommandResponse>()
            .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));

        // Update Komutu İçin Mapping
        CreateMap<Category, UpdateCategoryCommandResponse>()
             .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));

        CreateMap<Category, DeleteCategoryCommandResponse>();
    }
}
