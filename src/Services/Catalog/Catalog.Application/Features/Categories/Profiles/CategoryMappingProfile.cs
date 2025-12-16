using AutoMapper;
using BuildingBlocks.Core.Paging;
using Catalog.Application.Features.Categories.Commands.Create;
using Catalog.Application.Features.Categories.Commands.Delete;
using Catalog.Application.Features.Categories.Commands.Uptade;
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



        CreateMap<Category, UpdateCategoryCommandResponse>();



        CreateMap<Category, DeleteCategoryCommandResponse>();
    }
}
