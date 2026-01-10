using FluentValidation;

namespace Catalog.Application.Features.Categories.Commands.Uptade;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters");

        RuleFor(x => x.ParentCategoryId)
            .NotEqual(Guid.Empty)
            .When(x => x.ParentCategoryId.HasValue)
            .WithMessage("Parent Category Id cannot be empty.");

        RuleFor(x => x)
            .Must(x => x.ParentCategoryId != x.Id)
            .When(x => x.ParentCategoryId.HasValue)
            .WithMessage("A category cannot be its own parent.");
    }
}