using BuildingBlocks.Core.Responses;
using Identity.Application.Features.Auth.Login.Commands;
using Identity.Application.Features.Auth.Register.Commands;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        command.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var result = await Mediator.Send(command);

        var response = ApiResponse<RegisterCommandResponse>.SuccessResult(
            result,
            "User registered successfully"
        );

        return CreatedAtAction(nameof(Register), new { id = result.UserId }, response);
    }
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        command.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

        var result = await Mediator.Send(command);

        var response = ApiResponse<LoginCommandResponse>.SuccessResult(
            result,
            "Login successful"
        );

        return Ok(response);
    }
}
