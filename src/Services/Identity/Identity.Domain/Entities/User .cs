using BuildingBlocks.Core.Domain;
using Identity.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public class User : Entity<Guid>, IAggregateRoot
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool Status { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string RegistrationIp { get; set; } = string.Empty;
    public string? EmailConfirmationToken { get; set; }
    public DateTime? EmailConfirmationTokenExpiration { get; set; }  // ✅ EKLENDI

    // Navigation properties
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<UserOperationClaim> UserOperationClaims { get; set; }

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        UserName = string.Empty;
        Email = string.Empty;
        PasswordSalt = Array.Empty<byte>();
        PasswordHash = Array.Empty<byte>();
        RefreshTokens = new HashSet<RefreshToken>();
        UserOperationClaims = new HashSet<UserOperationClaim>();
        RegistrationIp = string.Empty;
    }

    public User(
        string firstName,
        string lastName,
        string email,
        string userName,
        byte[] passwordSalt,
        byte[] passwordHash,
        bool status,
        string registrationIp) : this()
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        Status = status;
        RegistrationIp = registrationIp;
        CreatedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory method: Yeni kullanıcı oluştur ve domain event fırlat
    /// </summary>
    public static User Create(
        string firstName,
        string lastName,
        string email,
        string userName,
        byte[] passwordSalt,
        byte[] passwordHash,
        string registrationIp,
        bool status = true)
    {
        var user = new User(firstName, lastName, email, userName, passwordSalt, passwordHash, status, registrationIp);

        var tokenBytes = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);
        user.EmailConfirmationToken = Convert.ToBase64String(tokenBytes);
        user.EmailConfirmationTokenExpiration = DateTime.UtcNow.AddHours(24);
        user.IsEmailConfirmed = false;

        user.AddDomainEvent(new UserCreatedDomainEvent(
            userId: user.Id,
            email: user.Email,
            firstName: user.FirstName,
            lastName: user.LastName,
            userName: user.UserName,
            emailConfirmationToken: user.EmailConfirmationToken 
        ));

        return user;
    }

    public void ConfirmEmail()
    {
        if (IsEmailConfirmed)
            throw new InvalidOperationException("Email is already confirmed");

        IsEmailConfirmed = true;
        EmailConfirmationToken = null;
        EmailConfirmationTokenExpiration = null;  
        UpdatedDate = DateTime.UtcNow;

        AddDomainEvent(new UserEmailConfirmedDomainEvent(
            userId: Id,
            email: Email
        ));
    }
}