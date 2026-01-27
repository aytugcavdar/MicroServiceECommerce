using BuildingBlocks.Core.Paging;
using BuildingBlocks.Core.Requests;
using BuildingBlocks.Core.Responses;
using Identity.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Orders.Commands.CreateOrder;
using Order.Application.Features.Orders.Queries.GetOrderById;
using Order.Application.Features.Orders.Queries.GetOrderStatistics;
using Order.Application.Features.Orders.Queries.GetUserOrders;
using BuildingBlocks.Security.Extensions;
using Order.Domain.Enums;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateOrderCommandResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        try
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.OrderId },
                ApiResponse<CreateOrderCommandResponse>.SuccessResult(
                    response,
                    "Sipariş başarıyla oluşturuldu"
                )
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Message = "Sunucu Hatası (Debug)",
                Error = ex.Message,
                Stack = ex.StackTrace,
                Inner = ex.InnerException?.Message
            });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetOrderByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetOrderByIdQuery
        {
            OrderId = id
        };

        var response = await Mediator.Send(query);

        return Ok(ApiResponse<GetOrderByIdResponse>.SuccessResult(response));
    }

    [HttpGet("my-orders")]
    [ProducesResponseType(typeof(ApiResponse<Paginate<GetUserOrdersListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyOrders(
        [FromQuery] PageRequest pageRequest,
        [FromQuery] OrderStatus? statusFilter = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        // ✅ Şimdi çalışacak
        var userIdString = User.GetUserId();

        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized(ApiResponse<object>.FailResult("User not authenticated", 401));

        var userId = Guid.Parse(userIdString);

        var query = new GetUserOrdersQuery
        {
            UserId = userId,
            PageRequest = pageRequest,
            StatusFilter = statusFilter,
            FromDate = fromDate,
            ToDate = toDate
        };

        var response = await Mediator.Send(query);
        return Ok(ApiResponse<Paginate<GetUserOrdersListItemDto>>.SuccessResult(response));
    }


}