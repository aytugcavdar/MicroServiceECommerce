using Catalog.Application.Features.Categories.Rules;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catalog.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryBusinessRules _categoryBusinessRules;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        CategoryBusinessRules categoryBusinessRules,
        ILogger<CreateCategoryCommandHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _categoryBusinessRules = categoryBusinessRules;
        _logger = logger;
    }

    public async Task<CreateCategoryCommandResponse> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("📝 Creating category: {Name} (Parent: {ParentId})",
            request.Name, request.ParentCategoryId);

        await _categoryBusinessRules.CategoryNameShouldBeUnique(
            request.Name,
            cancellationToken: cancellationToken);

        Category category = new Category(request.Name, request.ParentCategoryId);
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("✅ Category created successfully: {Id} - {Name}",
            category.Id, category.Name);

        return new CreateCategoryCommandResponse
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            CreatedDate = category.CreatedDate
        };
    }
}