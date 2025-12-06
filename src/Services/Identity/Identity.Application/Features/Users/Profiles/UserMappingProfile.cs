using AutoMapper;
using BuildingBlocks.Core.Paging;
using Identity.Application.Features.Users.Queries.GetListUser;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Users.Profiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, GetListUserListItemDto>();
        CreateMap<Paginate<User>, Paginate<GetListUserListItemDto>>();
    }
}
