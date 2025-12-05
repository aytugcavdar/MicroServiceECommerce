using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Services;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Register.Rules;

public class AuthBusinessRules
{
    private readonly IUserRepository _userRepository;
    private readonly IOperationClaimRepository _operationClaimRepository;

    public AuthBusinessRules(
        IUserRepository userRepository,
        IOperationClaimRepository operationClaimRepository)
    {
        _userRepository = userRepository;
        _operationClaimRepository = operationClaimRepository;
    }

    /// <summary>
    /// Email daha önce kullanılmış mı kontrol et
    /// </summary>
    public async Task EmailShouldNotExistWhenRegistering(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(
            predicate: u => u.Email == email,
            cancellationToken: cancellationToken);

        if (user != null)
        {
            throw new BusinessException("Email already exists");
        }
    }

    /// <summary>
    /// Username daha önce kullanılmış mı kontrol et
    /// </summary>
    public async Task UserNameShouldNotExistWhenRegistering(
        string userName,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(
            predicate: u => u.UserName == userName,
            cancellationToken: cancellationToken);

        if (user != null)
        {
            throw new BusinessException("Username already exists");
        }
    }

    /// <summary>
    /// User email'e göre bulunmalı
    /// </summary>
    public async Task<User> UserShouldExistWhenSelected(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(
            predicate: u => u.Email == email,
            cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User", email);
        }

        return user;
    }

    /// <summary>
    /// User ID'ye göre bulunmalı
    /// </summary>
    public async Task<User> UserShouldExistWhenSelected(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(
            predicate: u => u.Id == userId,
            cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User", userId);
        }

        return user;
    }

    /// <summary>
    /// Kullanıcının email'i doğrulanmış olmalı
    /// </summary>
    public void UserEmailShouldBeConfirmed(User user)
    {
        if (!user.IsEmailConfirmed)
        {
            throw new BusinessException("Email is not confirmed. Please check your email.");
        }
    }

    /// <summary>
    /// Kullanıcı aktif olmalı
    /// </summary>
    public void UserShouldBeActive(User user)
    {
        if (!user.Status)
        {
            throw new BusinessException("User account is inactive");
        }
    }

    /// <summary>
    /// Email confirmation token doğru olmalı
    /// </summary>
    public void EmailConfirmationTokenShouldBeValid(User user, string token)
    {
        if (user.EmailConfirmationToken != token)
        {
            throw new BusinessException("Invalid confirmation token");
        }
    }

    /// <summary>
    /// Email zaten doğrulanmamış olmalı
    /// </summary>
    public void EmailShouldNotBeAlreadyConfirmed(User user)
    {
        if (user.IsEmailConfirmed)
        {
            throw new BusinessException("Email is already confirmed");
        }
    }

    /// <summary>
    /// Şifre doğru olmalı
    /// </summary>
    public void PasswordShouldBeCorrect(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        var isCorrect = BuildingBlocks.Security.Encryption.HashingHelper.VerifyPasswordHash(
            password,
            passwordHash,
            passwordSalt);

        if (!isCorrect)
        {
            throw new BusinessException("Invalid email or password");
        }
    }

    /// <summary>
    /// Refresh token geçerli olmalı
    /// </summary>
    public void RefreshTokenShouldBeValid(RefreshToken refreshToken)
    {
        if (refreshToken.IsRevoked)
        {
            throw new BusinessException("Refresh token has been revoked");
        }

        if (refreshToken.IsExpired)
        {
            throw new BusinessException("Refresh token has expired");
        }
    }

    /// <summary>
    /// Default rol (User) var olmalı
    /// </summary>
    public async Task<OperationClaim> DefaultUserRoleShouldExist(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        var role = await _operationClaimRepository.GetAsync(
            predicate: c => c.Name == roleName,
            cancellationToken: cancellationToken);

        if (role == null)
        {
            throw new BusinessException($"Default role '{roleName}' not found in system");
        }

        return role;
    }
}