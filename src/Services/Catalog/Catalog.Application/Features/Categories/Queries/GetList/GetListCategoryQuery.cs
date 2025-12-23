using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using BuildingBlocks.Core.Requests;
using BuildingBlocks.Core.Paging;
namespace Catalog.Application.Features.Categories.Queries.GetList;

public class GetListCategoryQuery:IRequest<Paginate<GetListCategoryListItemDto>>
{
    public PageRequest PageRequest { get; set; }
    public GetListCategoryQuery()
    {
        PageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };
    }
    public GetListCategoryQuery(PageRequest pageRequest)
    {
        PageRequest = pageRequest;
    }
}
