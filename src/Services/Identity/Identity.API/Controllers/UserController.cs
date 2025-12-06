using BuildingBlocks.Core.Requests;
using Identity.Application.Features.Users.Queries.GetListUser;
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
}