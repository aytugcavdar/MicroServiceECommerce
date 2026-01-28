using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Catalog.Application.Constants;
using Catalog.Domain.Constants;
namespace Catalog.Application.Features.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        // ✅ Domain constants + Validation messages
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required)
            .MinimumLength(CatalogConstants.Product.NameMinLength)
                .WithMessage(ValidationMessages.MinLength)
            .MaximumLength(CatalogConstants.Product.NameMaxLength)
                .WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required)
            .MinimumLength(CatalogConstants.Product.DescriptionMinLength)
                .WithMessage(ValidationMessages.MinLength)
            .MaximumLength(CatalogConstants.Product.DescriptionMaxLength)
                .WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(CatalogConstants.Product.MinPrice)
                .WithMessage(ValidationMessages.GreaterThanOrEqual)
            .LessThanOrEqualTo(CatalogConstants.Product.MaxPrice)
                .WithMessage(ValidationMessages.LessThanOrEqual);

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(CatalogConstants.Product.MinStock)
                .WithMessage(ValidationMessages.Product.NegativeStock)
            .LessThanOrEqualTo(CatalogConstants.Product.MaxStock)
                .WithMessage(ValidationMessages.LessThanOrEqual);

        RuleFor(x => x.PictureFileName)
            .MaximumLength(CatalogConstants.Product.PictureFileNameMaxLength)
                .WithMessage(ValidationMessages.MaxLength)
            .When(x => !string.IsNullOrEmpty(x.PictureFileName));

        RuleFor(x => x.CategoryId)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required);
    }
}
