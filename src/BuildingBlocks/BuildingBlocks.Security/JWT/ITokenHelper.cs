using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Security.JWT;

public interface ITokenHelper
{

    AccessToken CreateToken(Guid userId, string email, string userName, List<string> roles);
    string CreateRefreshToken();
}
