using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.Application.Features.Baskets.Commands.Checkout;
using Basket.Application.Features.Baskets.Commands.DeleteBasket;
using Basket.Application.Features.Baskets.Commands.UpdateBasket;
using Basket.Application.Features.Baskets.Queries.GetBasket;
using BuildingBlocks.Core.Responses;
using BuildingBlocks.Messaging.IntegrationEvents;
using BuildingBlocks.Security.Extensions;
using Identity.API.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BasketController : BaseController
{
    [HttpGet("{userName}")]
    [ProducesResponseType(typeof(ApiResponse<GetBasketQueryResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasket(string userName)
    {
        var query = new GetBasketQuery { UserName = userName };
        var result = await Mediator.Send(query);

        return Ok(ApiResponse<GetBasketQueryResponse>.SuccessResult(result));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UpdateBasketCommandResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(ApiResponse<UpdateBasketCommandResponse>.SuccessResult(
            result,
            "Basket updated successfully"));
    }

    [HttpDelete("{userName}")]
    [ProducesResponseType(typeof(ApiResponse<DeleteBasketCommandResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        var command = new DeleteBasketCommand { UserName = userName };
        var result = await Mediator.Send(command);

        return Ok(ApiResponse<DeleteBasketCommandResponse>.SuccessResult(
            result,
            "Basket deleted successfully"));
    }

    [HttpPost("checkout")]
    [ProducesResponseType(typeof(ApiResponse<CheckoutBasketCommandResponse>), (int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] CheckoutBasketCommand command)
    {
        var result = await Mediator.Send(command);

        return Accepted(ApiResponse<CheckoutBasketCommandResponse>.SuccessResult(
            result,
            "Checkout completed successfully"));
    }
}
