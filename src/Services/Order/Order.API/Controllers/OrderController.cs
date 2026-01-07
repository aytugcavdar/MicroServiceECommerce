using Identity.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Orders.Commands.CreateOrder;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand createOrderCommand)
    {
        CreateOrderCommandResponse response = await Mediator.Send(createOrderCommand);
        return Ok(response);
    }
}
