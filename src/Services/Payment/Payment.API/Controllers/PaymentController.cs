using BuildingBlocks.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Features.Payments.Queries.GetPaymentByOrderId;

namespace Payment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : BaseController
{
    /// <summary>
    /// Get payment details by order ID
    /// </summary>
    [HttpGet("order/{orderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetPaymentByOrderIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByOrderId(Guid orderId)
    {
        var query = new GetPaymentByOrderIdQuery { OrderId = orderId };
        var response = await Mediator.Send(query);

        if (!response.Found)
        {
            return NotFound(ApiResponse<object>.FailResult(
                $"Payment not found for order {orderId}", 
                StatusCodes.Status404NotFound));
        }

        return Ok(ApiResponse<GetPaymentByOrderIdResponse>.SuccessResult(response));
    }

    /// <summary>
    /// Health check endpoint for payment gateway
    /// </summary>
    [HttpGet("gateway-status")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetGatewayStatus()
    {
        return Ok(new
        {
            gateway = "MockPaymentGateway",
            status = "Operational",
            timestamp = DateTime.UtcNow,
            message = "Mock gateway for demonstration purposes. 80% success rate."
        });
    }
}
