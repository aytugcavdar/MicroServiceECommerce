using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Login.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email or username is required")
            .MinimumLength(3).WithMessage("Email or username must be at least 3 characters")
            .MaximumLength(100).WithMessage("Email or username cannot exceed 100 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}
