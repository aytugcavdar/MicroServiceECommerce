using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Services;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.Register.Rules;

public class RegisterBusinessRules
{
    private readonly IUserRepository _userRepository;
    private readonly IOperationClaimRepository _roleRepository;

    public RegisterBusinessRules(IUserRepository userRepository, IOperationClaimRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    // 1. Email tekrarı kontrolü
    public async Task EmailShouldNotExistWhenRegistering(string email)
    {
        var user = await _userRepository.GetAsync(u => u.Email == email);
        if (user != null) throw new BusinessException("Email already exists");
    }

    // 2. Kullanıcı adı tekrarı kontrolü
    public async Task UserNameShouldNotExistWhenRegistering(string userName)
    {
        var user = await _userRepository.GetAsync(u => u.UserName == userName);
        if (user != null) throw new BusinessException("Username already exists");
    }

    // 3. Varsayılan rol kontrolü
    public async Task<OperationClaim> DefaultUserRoleShouldExist(string roleName)
    {
        var role = await _roleRepository.GetAsync(c => c.Name == roleName);
        if (role == null) throw new BusinessException($"Default role '{roleName}' not found");
        return role;
    }
}