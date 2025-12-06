using BuildingBlocks.Core.Security.Constants;
using BuildingBlocks.CrossCutting.Exceptions.types;
using BuildingBlocks.Security.Encryption;
using Identity.Application.Features.Auth.Register.Rules;
using Identity.Application.Services;
using Identity.Domain.Entities;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Register.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly Serilog.ILogger _logger;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        AuthBusinessRules authBusinessRules)
    {
        _userRepository = userRepository;
        _authBusinessRules = authBusinessRules;
        _logger = Log.ForContext<RegisterCommandHandler>();
    }

    public async Task<RegisterCommandResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        _logger.Information(
            "📝 Registration attempt for email: {Email}",
            request.Email);

        // ============================================
        // 1. BUSINESS RULES VALIDATION
        // ============================================
        await _authBusinessRules.EmailShouldNotExistWhenRegistering(
            request.Email,
            cancellationToken);

        await _authBusinessRules.UserNameShouldNotExistWhenRegistering(
            request.UserName,
            cancellationToken);

        var defaultRole = await _authBusinessRules.DefaultUserRoleShouldExist(
            GeneralOperationClaims.User,
            cancellationToken);

        // ============================================
        // 2. PASSWORD HASHING
        // ============================================
        HashingHelper.CreatePasswordHash(
            request.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

        // ============================================
        // 3. CREATE USER (Domain Logic)
        // ============================================
        User user = User.Create(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            userName: request.UserName,
            passwordSalt: passwordSalt,
            passwordHash: passwordHash,
            registrationIp: request.IpAddress);

        // ============================================
        // 4. ASSIGN DEFAULT ROLE
        // ============================================
        user.UserOperationClaims.Add(
            new UserOperationClaim(user.Id, defaultRole.Id));

        _logger.Debug(
            "👤 Default role assigned: {RoleName}",
            GeneralOperationClaims.User);

        // ============================================
        // 5. SAVE TO DATABASE
        // ============================================
        await _userRepository.AddAsync(user);

        _logger.Information(
            "✅ User registered successfully: {UserId} - {Email}",
            user.Id,
            user.Email);

        // ============================================
        // 6. RETURN RESPONSE
        // ============================================
        return new RegisterCommandResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Message = "Registration successful. Please check your email to confirm your account."
        };
    }
}