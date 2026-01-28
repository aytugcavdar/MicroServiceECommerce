using FluentValidation;
using Basket.Application.Constants;
using Basket.Domain.Constants;

namespace Basket.Application.Features.Baskets.Commands.Checkout;

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(50).WithMessage(ValidationMessages.MaxLength); // Should migrate these magic numbers too eventually

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .EmailAddress().WithMessage(ValidationMessages.InvalidFormat);

        RuleFor(x => x.AddressLine)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .CreditCard().WithMessage("Invalid card number"); // Could add specific message for CreditCard

        RuleFor(x => x.CVV)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits"); // Specific regex message
    }
}