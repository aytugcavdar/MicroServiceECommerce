using FluentValidation;
using Basket.Application.Constants;
using Basket.Domain.Constants;

namespace Basket.Application.Features.Baskets.Commands.UpdateBasket;

public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
                .WithMessage(ValidationMessages.Required)
            .MaximumLength(100) // This magic number should probably be in constants too if not already
                .WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Items)
            .NotNull()
                .WithMessage(ValidationMessages.Required);

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .NotEmpty()
                    .WithMessage(ValidationMessages.Required);

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                    .WithMessage(ValidationMessages.GreaterThan)
                .LessThanOrEqualTo(BasketConstants.CartItem.MaxQuantity)
                    .WithMessage(ValidationMessages.CartItem.QuantityOutOfBounds);

            item.RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                    .WithMessage(ValidationMessages.GreaterThanOrEqual);

            item.RuleFor(x => x.ProductName)
                .NotEmpty()
                    .WithMessage(ValidationMessages.Required)
                .MaximumLength(BasketConstants.CartItem.ProductNameMaxLength)
                     .WithMessage(ValidationMessages.MaxLength);
        });
    }
}