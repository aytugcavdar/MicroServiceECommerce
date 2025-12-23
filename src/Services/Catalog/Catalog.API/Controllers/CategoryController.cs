using BuildingBlocks.Core.Requests;
using BuildingBlocks.Core.Responses;
using Catalog.Application.Features.Categories.Commands.Create;
using Catalog.Application.Features.Categories.Commands.Delete;
using Catalog.Application.Features.Categories.Commands.Uptade;
using Catalog.Application.Features.Categories.Queries;
using Catalog.Application.Features.Categories.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : BaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateCategoryCommandResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var result = await Mediator.Send(command);

        var response = ApiResponse<CreateCategoryCommandResponse>.SuccessResult(
            result,
            "Category created successfully"
        );

        return CreatedAtAction(nameof(Create), new { id = result.Id }, response);
    }
    [HttpGet]
    public async Task <IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListCategoryQuery query = new() {PageRequest = pageRequest};
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<DeleteCategoryCommandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await Mediator.Send(command);
        return Ok(ApiResponse<DeleteCategoryCommandResponse>.SuccessResult(result));
    }

    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<UpdateCategoryCommandResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(ApiResponse<UpdateCategoryCommandResponse>.SuccessResult(result));
    }

    [HttpGet("tree")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryTreeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTree([FromQuery] bool includeInactive = false)
    {
        var query = new GetCategoryTreeQuery { IncludeInactive = includeInactive };
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
