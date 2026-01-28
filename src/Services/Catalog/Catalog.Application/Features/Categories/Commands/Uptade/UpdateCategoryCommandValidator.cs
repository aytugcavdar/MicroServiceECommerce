using FluentValidation;
using Catalog.Application.Constants;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Categories.Commands.Uptade;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required)
            .MinimumLength(CatalogConstants.Category.NameMinLength)
                .WithMessage(ValidationMessages.MinLength)
            .MaximumLength(CatalogConstants.Category.NameMaxLength)
                .WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.ParentCategoryId)
            .NotEqual(Guid.Empty)
                .When(x => x.ParentCategoryId.HasValue)
                .WithMessage("Parent Category Id cannot be empty.");

        RuleFor(x => x)
            .Must(x => x.ParentCategoryId != x.Id)
                .When(x => x.ParentCategoryId.HasValue)
                .WithMessage(ValidationMessages.Category.CircularReference);
    }
}