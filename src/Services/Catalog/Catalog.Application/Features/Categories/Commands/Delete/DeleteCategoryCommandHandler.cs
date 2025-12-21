using Catalog.Application.Features.Categories.Rules;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryCommandResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryBusinessRules _categoryBusinessRules;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, CategoryBusinessRules categoryBusinessRules)
    {
        _categoryRepository = categoryRepository;
        _categoryBusinessRules = categoryBusinessRules;
    }

    public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = await _categoryBusinessRules.CategoryShouldExist(request.Id, cancellationToken);

        
        await _categoryBusinessRules.CategoryShouldNotHaveProducts(request.Id, cancellationToken);

        
        await _categoryRepository.DeleteAsync(category);

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return new DeleteCategoryCommandResponse(category.Id);
    }
}
