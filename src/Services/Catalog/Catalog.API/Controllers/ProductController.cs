using BuildingBlocks.Core.Requests;
using BuildingBlocks.Core.Responses;
using Catalog.Application.Features.Products.Commands.Create;
using Catalog.Application.Features.Products.Commands.Delete;
using Catalog.Application.Features.Products.Commands.Update;
using Catalog.Application.Features.Products.Queries.GetListProduct;
using Catalog.Application.Features.Products.Queries.GetByIdProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<CreateProductCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Add([FromBody] CreateProductCommand command)
    {
        var result = await Mediator.Send(command);

        var response = ApiResponse<CreateProductCommandResponse>.SuccessResult(
            result,
            "Product created successfully"
        );

        return StatusCode(StatusCodes.Status201Created, response);
    }


    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListProductQuery query = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetByIdProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var query = new GetByIdProductQuery(id);
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
