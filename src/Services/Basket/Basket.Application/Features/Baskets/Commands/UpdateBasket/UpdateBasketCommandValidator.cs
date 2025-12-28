using FluentValidation;

namespace Basket.Application.Features.Baskets.Commands.UpdateBasket;

public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required")
            .MaximumLength(100);

        RuleFor(x => x.Items)
            .NotNull().WithMessage("Items cannot be null");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100");

            item.RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");

            item.RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("ProductName is required");
        });
    }
}