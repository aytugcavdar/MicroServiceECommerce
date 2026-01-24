using BuildingBlocks.Core.Requests;
using BuildingBlocks.Core.Responses;
using BuildingBlocks.Security.Extensions;
using Identity.Application.Features.Users.Queries.GetListUser;
using Identity.Application.Features.Users.Queries.GetUserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        var query = new GetListUserQuery(pageRequest);
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponse<GetUserProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile()
    {
        var userIdString = User.GetUserId();

        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized(ApiResponse<object>.FailResult(
                "User not authenticated", 401));
        }

        var userId = Guid.Parse(userIdString);

        var query = new GetUserProfileQuery { UserId = userId };
        var result = await Mediator.Send(query);

        return Ok(ApiResponse<GetUserProfileResponse>.SuccessResult(
            result,
            "Profile retrieved successfully"));
    }


    [HttpGet("{userId}/profile")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<GetUserProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserProfile(Guid userId)
    {
        var query = new GetUserProfileQuery { UserId = userId };
        var result = await Mediator.Send(query);

        return Ok(ApiResponse<GetUserProfileResponse>.SuccessResult(
            result,
            "User profile retrieved successfully"));
    }
}