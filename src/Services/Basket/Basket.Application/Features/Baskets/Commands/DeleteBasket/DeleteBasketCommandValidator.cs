using FluentValidation;

namespace Basket.Application.Features.Baskets.Commands.DeleteBasket;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName boş olamaz.")
            .MinimumLength(2).WithMessage("UserName en az 2 karakter olmalıdır.");
    }
}
