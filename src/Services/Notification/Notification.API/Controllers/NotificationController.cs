using BuildingBlocks.Core.Requests;
using BuildingBlocks.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Features.Notifications.Queries.GetNotificationHistory;

namespace Notification.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : BaseController
{
    /// <summary>
    /// Belirli bir email için bildirim geçmişini getirir
    /// </summary>
    [HttpGet("history/{email}")]
    [ProducesResponseType(typeof(ApiResponse<List<GetNotificationHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(string email, [FromQuery] PageRequest pageRequest)
    {
        var query = new GetNotificationHistoryQuery 
        { 
            Email = email,
            PageRequest = pageRequest
        };
        
        var result = await Mediator.Send(query);
        
        return Ok(ApiResponse<List<GetNotificationHistoryResponse>>.SuccessResult(result));
    }

    /// <summary>
    /// Tüm bildirimleri listeler (Admin için)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<GetNotificationHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pageRequest)
    {
        var query = new GetNotificationHistoryQuery 
        { 
            Email = null,
            PageRequest = pageRequest
        };
        
        var result = await Mediator.Send(query);
        
        return Ok(ApiResponse<List<GetNotificationHistoryResponse>>.SuccessResult(result));
    }
}
