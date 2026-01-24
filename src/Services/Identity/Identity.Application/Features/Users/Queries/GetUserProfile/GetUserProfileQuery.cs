using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<GetUserProfileResponse>
{
    public Guid UserId { get; set; }
}


