using BuildingBlocks.Core.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Queries.Tree;

public class GetCategoryTreeQuery : IRequest<ApiResponse<List<CategoryTreeDto>>>
{
    public bool IncludeInactive { get; set; } = false;
}
