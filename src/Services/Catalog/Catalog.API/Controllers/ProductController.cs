using BuildingBlocks.Core.Requests;
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
}
