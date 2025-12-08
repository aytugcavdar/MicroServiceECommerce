using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Services;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Users.Rules;

public class UserBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UserBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> UserShouldExist(string email)
    {
        var user = await _userRepository.GetByEmailWithRolesAsync(email);
        if (user == null) throw new NotFoundException("User", email);
        return user;
    }

    // ID overload'ı
    public async Task<User> UserShouldExist(Guid id)
    {
        var user = await _userRepository.GetAsync(u => u.Id == id);
        if (user == null) throw new NotFoundException("User", id);
        return user;
    }
}