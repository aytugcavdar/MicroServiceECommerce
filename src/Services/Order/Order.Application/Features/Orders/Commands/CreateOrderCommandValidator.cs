using FluentValidation;
using Order.Application.Features.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Address is required")
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item")
            .Must(items => items != null && items.Count > 0)
            .WithMessage("Order items cannot be empty");

        RuleForEach(x => x.OrderItems)
            .SetValidator(new CreateOrderItemDtoValidator());
    }
}
