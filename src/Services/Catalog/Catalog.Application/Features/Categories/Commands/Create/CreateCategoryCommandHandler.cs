using Catalog.Application.Features.Categories.Rules;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
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
    private readonly Serilog.ILogger _logger;

    public async Task<CreateCategoryCommandResponse> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        // Loglama mesajını güncelledik
        _logger.Information("📝 Creating category: {Name} (Parent: {ParentId})", request.Name, request.ParentCategoryId);

        await _categoryBusinessRules.CategoryNameShouldBeUnique(
            request.Name,
            cancellationToken: cancellationToken
        );

       
        Category category = new Category(request.Name, request.ParentCategoryId);

        await _categoryRepository.AddAsync(category);

        _logger.Information(
            "✅ Category created successfully: {Id} - {Name}",
            category.Id,
            category.Name
        );

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return new CreateCategoryCommandResponse
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            CreatedDate = category.CreatedDate
        };
    }
}