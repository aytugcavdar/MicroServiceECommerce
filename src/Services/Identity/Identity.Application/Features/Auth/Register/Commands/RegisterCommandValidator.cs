using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using Identity.Application.Constants;
using Identity.Domain.Constants;
namespace Identity.Application.Features.Auth.Register.Commands;

public class RegisterCommandValidator:AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .EmailAddress().WithMessage(ValidationMessages.InvalidFormat)
            .MaximumLength(100).WithMessage(ValidationMessages.MaxLength);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MinimumLength(3).WithMessage(ValidationMessages.MinLength)
            .MaximumLength(50).WithMessage(ValidationMessages.MaxLength)
            .Matches("^[a-zA-Z0-9_]*$").WithMessage("Username can only contain letters, numbers and underscore");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Required)
            .MinimumLength(6).WithMessage(ValidationMessages.MinLength)
            .Matches("[A-Z]").WithMessage(ValidationMessages.User.PasswordUppercase)
            .Matches("[a-z]").WithMessage(ValidationMessages.User.PasswordLowercase)
            .Matches("[0-9]").WithMessage(ValidationMessages.User.PasswordNumber);
    }
}
