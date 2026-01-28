using FluentValidation;
using Catalog.Application.Constants;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Products.Commands.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();

        RuleFor(p => p.Name)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required)
            .MinimumLength(CatalogConstants.Product.NameMinLength)
                .WithMessage(ValidationMessages.MinLength)
            .MaximumLength(CatalogConstants.Product.NameMaxLength)
                .WithMessage(ValidationMessages.MaxLength);

        RuleFor(p => p.Description)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required)
            .MinimumLength(CatalogConstants.Product.DescriptionMinLength)
                .WithMessage(ValidationMessages.MinLength)
            .MaximumLength(CatalogConstants.Product.DescriptionMaxLength)
                .WithMessage(ValidationMessages.MaxLength);

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(CatalogConstants.Product.MinPrice)
                .WithMessage(ValidationMessages.GreaterThanOrEqual)
            .LessThanOrEqualTo(CatalogConstants.Product.MaxPrice)
                .WithMessage(ValidationMessages.LessThanOrEqual);

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(CatalogConstants.Product.MinStock)
                .WithMessage(ValidationMessages.Product.NegativeStock)
            .LessThanOrEqualTo(CatalogConstants.Product.MaxStock)
                .WithMessage(ValidationMessages.LessThanOrEqual);

        RuleFor(p => p.CategoryId)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required);
    }
}
