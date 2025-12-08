using BuildingBlocks.CrossCutting.Exceptions.types;
using BuildingBlocks.Security.Encryption;
using Identity.Application.Services;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Login.Rules;

public class LoginBusinessRules
{
    private readonly IUserRepository _userRepository;

    public LoginBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Kullanıcı email veya username ile bulunmalı
    /// </summary>
    public async Task<User> UserShouldExistByEmailOrUsername(string emailOrUsername)
    {
        var user = await _userRepository.GetByEmailWithRolesAsync(emailOrUsername)
                   ?? await _userRepository.GetByUserNameWithRolesAsync(emailOrUsername);

        if (user == null)
            throw new BusinessException("Invalid email/username or password");

        return user;
    }

    /// <summary>
    /// Email doğrulanmış olmalı
    /// </summary>
    public void UserEmailShouldBeConfirmed(User user)
    {
        if (!user.IsEmailConfirmed)
            throw new BusinessException("Email is not confirmed. Please check your inbox.");
    }

    /// <summary>
    /// Kullanıcı hesabı aktif olmalı
    /// </summary>
    public void UserAccountShouldBeActive(User user)
    {
        if (!user.Status)
            throw new BusinessException("Your account has been disabled. Please contact support.");
    }

    /// <summary>
    /// Şifre doğru olmalı
    /// </summary>
    public void PasswordShouldBeCorrect(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        bool isPasswordCorrect = HashingHelper.VerifyPasswordHash(
            password,
            passwordHash,
            passwordSalt
        );

        if (!isPasswordCorrect)
            throw new BusinessException("Invalid email/username or password");
    }
}