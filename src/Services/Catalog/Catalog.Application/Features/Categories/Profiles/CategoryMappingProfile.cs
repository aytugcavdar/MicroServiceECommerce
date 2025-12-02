using AutoMapper;
using BuildingBlocks.Core.Paging;
using Catalog.Application.Features.Categories.Commands;
using Catalog.Application.Features.Categories.Queries;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Profiles;

public class CategoryMappingProfile:Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, GetListCategoryListItemDto>();

        CreateMap<Paginate<Category>, Paginate<GetListCategoryListItemDto>>();

        CreateMap<Category, CreateCategoryCommandResponse>();
    }
}
