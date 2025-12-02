using Identity.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Register.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IOperationClaimRepository _operationClaimRepository;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IOperationClaimRepository operationClaimRepository)
    {
        _userRepository = userRepository;
        _operationClaimRepository = operationClaimRepository;
    }

    public async Task<RegisterCommandResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        // Email kontrolü
        var existingUser = await _userRepository.GetAsync(
            u => u.Email == request.Email,
            cancellationToken: cancellationToken);

        if (existingUser != null)
            throw new BusinessException("Email already exists");

        // Username kontrolü
        existingUser = await _userRepository.GetAsync(
            u => u.UserName == request.UserName,
            cancellationToken: cancellationToken);

        if (existingUser != null)
            throw new BusinessException("Username already exists");

        // Password hash
        HashingHelper.CreatePasswordHash(
            request.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt);

        // User oluştur
        User user = new(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            userName: request.UserName,
            passwordSalt: passwordSalt,
            passwordHash: passwordHash,
            status: true);

        // Default "User" rolünü ekle
        var userClaim = await _operationClaimRepository.GetAsync(
            c => c.Name == "User",
            cancellationToken: cancellationToken);

        if (userClaim != null)
        {
            user.UserOperationClaims.Add(new UserOperationClaim(user.Id, userClaim.Id));
        }

        await _userRepository.AddAsync(user);

        return new RegisterCommandResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Message = "Registration successful. Please check your email to confirm your account."
        };
    }
}