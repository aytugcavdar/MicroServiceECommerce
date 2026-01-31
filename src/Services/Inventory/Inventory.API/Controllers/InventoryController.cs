using BuildingBlocks.Core.Responses;
using Inventory.Application.Features.Inventory.Commands.AdjustStock;
using Inventory.Application.Features.Inventory.Queries.GetInventoryByProductId;
using Inventory.Application.Features.Inventory.Queries.GetLowStock;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : BaseController
{
    /// <summary>
    /// Belirli bir ürünün stok bilgisini getirir
    /// </summary>
    [HttpGet("{productId}")]
    [ProducesResponseType(typeof(ApiResponse<GetInventoryByProductIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProductId(Guid productId)
    {
        var query = new GetInventoryByProductIdQuery { ProductId = productId };
        var result = await Mediator.Send(query);
        
        return Ok(ApiResponse<GetInventoryByProductIdResponse>.SuccessResult(result));
    }

    /// <summary>
    /// Düşük stoklu ürünleri listeler
    /// </summary>
    [HttpGet("low-stock")]
    [ProducesResponseType(typeof(ApiResponse<List<GetLowStockResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
    {
        var query = new GetLowStockQuery { Threshold = threshold };
        var result = await Mediator.Send(query);
        
        return Ok(ApiResponse<List<GetLowStockResponse>>.SuccessResult(result));
    }

    /// <summary>
    /// Stok miktarını günceller (artırma veya azaltma)
    /// </summary>
    [HttpPut("{productId}/adjust")]
    [ProducesResponseType(typeof(ApiResponse<AdjustStockCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AdjustStock(Guid productId, [FromBody] AdjustStockRequest request)
    {
        var command = new AdjustStockCommand
        {
            ProductId = productId,
            Quantity = request.Quantity,
            Reason = request.Reason
        };
        
        var result = await Mediator.Send(command);
        
        return Ok(ApiResponse<AdjustStockCommandResponse>.SuccessResult(
            result,
            "Stok başarıyla güncellendi"));
    }
}

public record AdjustStockRequest(int Quantity, string? Reason);
