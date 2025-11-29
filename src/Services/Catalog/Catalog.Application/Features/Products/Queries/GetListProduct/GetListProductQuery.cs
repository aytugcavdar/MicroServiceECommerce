using BuildingBlocks.Core.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using BuildingBlocks.Core.Requests;

namespace Catalog.Application.Features.Products.Queries.GetListProduct;

public class GetListProductQuery : IRequest<Paginate<GetListProductListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public GetListProductQuery()
    {
        PageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };
    }

    public GetListProductQuery(PageRequest pageRequest)
    {
        PageRequest = pageRequest;
    }
}
