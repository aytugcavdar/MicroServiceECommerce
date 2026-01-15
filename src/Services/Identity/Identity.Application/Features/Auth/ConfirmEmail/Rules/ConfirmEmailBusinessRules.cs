using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.ConfirmEmail.Rules;

public class ConfirmEmailBusinessRules
{
    // 1. Token doğru mu?
    public void EmailConfirmationTokenShouldBeValid(User user, string token)
    {
        if (user.EmailConfirmationToken != token)
            throw new BusinessException("Invalid confirmation token");
    }

    // 2. Zaten doğrulanmış mı?
    public void EmailShouldNotBeAlreadyConfirmed(User user)
    {
        if (user.IsEmailConfirmed)
            throw new BusinessException("Email is already confirmed");
    }

    public void EmailConfirmationTokenShouldNotBeExpired(User user)
    {
        if (user.EmailConfirmationTokenExpiration.HasValue &&
            user.EmailConfirmationTokenExpiration.Value < DateTime.UtcNow)
        {
            throw new BusinessException(
                "Confirmation token has expired. Please request a new confirmation email.");
        }
    }
}