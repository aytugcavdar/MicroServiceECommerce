using FluentValidation;
using Order.Application.Constants;
using Order.Domain.Constants;

namespace Order.Application.Features.Orders.Dtos;

public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(ValidationMessages.GreaterThan);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(ValidationMessages.GreaterThan)
            .LessThanOrEqualTo(100).WithMessage(ValidationMessages.LessThanOrEqual); // Should consider moving 100 to OrderConstants
    }
}
