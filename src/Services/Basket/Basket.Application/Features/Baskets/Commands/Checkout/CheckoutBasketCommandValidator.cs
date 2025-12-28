using FluentValidation;

namespace Basket.Application.Features.Baskets.Commands.Checkout;

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required")
            .MaximumLength(50);

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.AddressLine)
            .NotEmpty().WithMessage("Address is required");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required");

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required")
            .CreditCard().WithMessage("Invalid card number");

        RuleFor(x => x.CVV)
            .NotEmpty().WithMessage("CVV is required")
            .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits");
    }
}