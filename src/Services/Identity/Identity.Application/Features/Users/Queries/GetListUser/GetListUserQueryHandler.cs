using AutoMapper;
using BuildingBlocks.Core.Paging;
using Identity.Application.Services;
using Identity.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Users.Queries.GetListUser;

public class GetListUserQueryHandler : IRequestHandler<GetListUserQuery, Paginate<GetListUserListItemDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetListUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Paginate<GetListUserListItemDto>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
    {
        
        Paginate<User> users = await _userRepository.GetListAsync(
            index: request.PageRequest.PageIndex,
            size: request.PageRequest.PageSize,
            cancellationToken: cancellationToken
        );

        
        Paginate<GetListUserListItemDto> mappedUserList = _mapper.Map<Paginate<GetListUserListItemDto>>(users);

        return mappedUserList;
    }
}
