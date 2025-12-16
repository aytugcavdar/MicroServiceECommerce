using BuildingBlocks.Core.Requests;
using BuildingBlocks.Core.Responses;
using Catalog.Application.Features.Products.Commands.Delete;
using Catalog.Application.Features.Products.Commands.Update;
using Catalog.Application.Features.Products.Queries.GetListProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListProductQuery query = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<UpdateProductCommandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
    {
        var result = await Mediator.Send(command);

        var response = ApiResponse<UpdateProductCommandResponse>.SuccessResult(
            result,
            "Product updated successfully"
        );

        return Ok(response);
    }

    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<DeleteProductCommandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteProductCommand(id);
        var result = await Mediator.Send(command);

        var response = ApiResponse<DeleteProductCommandResponse>.SuccessResult(
            result,
            "Product deleted successfully"
        );

        return Ok(response);
    }

}
