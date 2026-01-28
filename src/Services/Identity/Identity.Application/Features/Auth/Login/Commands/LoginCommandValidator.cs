using FluentValidation;
using Identity.Application.Constants;
using Identity.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Login.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MinimumLength(3).WithMessage(ValidationMessages.MinLength)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MinimumLength(6).WithMessage(ValidationMessages.MinLength);
    }
}
