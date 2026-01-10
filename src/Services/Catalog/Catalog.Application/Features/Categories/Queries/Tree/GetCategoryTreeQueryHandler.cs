using BuildingBlocks.Core.Responses;
using Catalog.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Categories.Queries.Tree;

public class GetCategoryTreeQueryHandler : IRequestHandler<GetCategoryTreeQuery, ApiResponse<List<CategoryTreeDto>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryTreeQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiResponse<List<CategoryTreeDto>>> Handle(
        GetCategoryTreeQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.Query()
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .Where(c => request.IncludeInactive || c.IsActive)
            .ToListAsync(cancellationToken);

        var rootCategories = categories
            .Where(c => c.ParentCategoryId == null)
            .Select(c => MapToDto(c, categories))
            .ToList();

        return ApiResponse<List<CategoryTreeDto>>.SuccessResult(
            rootCategories,
            "Category tree retrieved successfully"
        );
    }

    private CategoryTreeDto MapToDto(
        Domain.Entities.Category category,
        List<Domain.Entities.Category> allCategories)
    {
        return new CategoryTreeDto
        {
            Id = category.Id,
            Name = category.Name,
            IsActive = category.IsActive,
            ProductCount = category.Products?.Count ?? 0,
            SubCategories = allCategories
                .Where(c => c.ParentCategoryId == category.Id)
                .Select(c => MapToDto(c, allCategories))
                .ToList()
        };
    }
}