using BuildingBlocks.CrossCutting.Exceptions.types;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Catalog.Application.Constants;
using Catalog.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Rules;

public class CategoryBusinessRules
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryBusinessRules(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task CategoryNameShouldBeUnique(
        string name,
        Guid? excludeCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var existingCategory = await _categoryRepository.GetAsync(
             c => c.Name.ToLower() == name.ToLower() &&
                  (excludeCategoryId == null || c.Id != excludeCategoryId),
             cancellationToken: cancellationToken
         );

        if (existingCategory != null)
            throw new BusinessException(ValidationMessages.Category.NameAlreadyExists);
    }

    public async Task CategoryShouldNotHaveProducts(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var productCount = await _categoryRepository.GetProductCountAsync(
            categoryId, cancellationToken);

        if (productCount > 0)
            throw new BusinessException(
                ValidationMessages.Category.HasProducts
                    .Replace("{ProductCount}", productCount.ToString()));
    }
    public async Task<Category> CategoryShouldExist(
       Guid categoryId,
       CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetAsync(
            c => c.Id == categoryId,
            cancellationToken: cancellationToken
        );

        if (category == null)
            throw new NotFoundException("Category", categoryId);

        return category;
    }
}
