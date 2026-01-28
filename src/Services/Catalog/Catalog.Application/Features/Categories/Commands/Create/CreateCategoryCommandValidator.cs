using FluentValidation;
using Catalog.Application.Constants;
using Catalog.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
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
    }
}