using Catalog.Application.Features.Categories.Rules;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Commands.Uptade;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryBusinessRules _categoryBusinessRules;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, CategoryBusinessRules categoryBusinessRules)
    {
        _categoryRepository = categoryRepository;
        _categoryBusinessRules = categoryBusinessRules;
    }

    public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category =await _categoryBusinessRules.CategoryShouldExist(request.Id, cancellationToken);

        await _categoryBusinessRules.CategoryNameShouldBeUnique(request.Name, excludeCategoryId: request.Id, cancellationToken: cancellationToken);

        category.Name = request.Name;

        await _categoryRepository.UpdateAsync(category);

        return new UpdateCategoryCommandResponse(category.Id, category.Name, category.UpdatedDate ?? DateTime.UtcNow);
    }
}
