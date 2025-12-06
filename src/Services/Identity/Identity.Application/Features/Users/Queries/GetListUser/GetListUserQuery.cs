using BuildingBlocks.Core.Paging;
using BuildingBlocks.Core.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Users.Queries.GetListUser;

public class GetListUserQuery : IRequest<Paginate<GetListUserListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public GetListUserQuery()
    {
        PageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };
    }

    public GetListUserQuery(PageRequest pageRequest)
    {
        PageRequest = pageRequest;
    }
}
