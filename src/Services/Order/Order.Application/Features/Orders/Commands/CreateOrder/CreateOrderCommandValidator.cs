using FluentValidation;
using Order.Application.Features.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

using Order.Application.Constants;
using Order.Domain.Constants;
namespace Order.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ValidationMessages.Required);

        RuleFor(x => x.Address)
            .NotNull().WithMessage(ValidationMessages.Required)
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .Must(items => items != null && items.Count > 0)
            .WithMessage(ValidationMessages.Order.TooManyItems); // Reusing this but maybe needs "AtLeastOne"

        RuleForEach(x => x.OrderItems)
            .SetValidator(new CreateOrderItemDtoValidator());
    }
}
