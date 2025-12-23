using AutoMapper;
using BuildingBlocks.Core.Paging;
using Catalog.Application.Features.Products.Queries.GetListProduct;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Queries.GetList;

public class GetListCategoryQueryHandler:IRequestHandler<GetListCategoryQuery,Paginate<GetListCategoryListItemDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetListCategoryQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Paginate<GetListCategoryListItemDto>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
    {
        Paginate<Category> category = await _categoryRepository.GetListAsync(
            index:request.PageRequest.PageIndex,
            size:request.PageRequest.PageSize,
            cancellationToken:cancellationToken
            );

        Paginate<GetListCategoryListItemDto> mappedCategoryList = _mapper.Map<Paginate<GetListCategoryListItemDto>>(category);

        return mappedCategoryList;
    
    }
}
