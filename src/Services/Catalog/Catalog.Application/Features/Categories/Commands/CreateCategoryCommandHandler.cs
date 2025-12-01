using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catalog.Application.Features.Categories.Commands;

public class CreateCategoryCommandHandler: IRequestHandler<CreateCategoryCommand,CreateCategoryCommandResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = new Category
            (
                request.Name
            );
        await _categoryRepository.AddAsync(category);

        return new CreateCategoryCommandResponse
        {
            Id = category.Id,
            Name = category.Name,
            CreatedDate = category.CreatedDate
        };

    }
}
