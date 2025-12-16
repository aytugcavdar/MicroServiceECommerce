using FluentValidation;

namespace Catalog.Application.Features.Products.Commands.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty();

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Product description is required")
            .MaximumLength(500).WithMessage("Product description cannot exceed 500 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");

        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("Category is required");
    }
}
