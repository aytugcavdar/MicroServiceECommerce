using BuildingBlocks.Core.Domain;
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

    // BCrypt için tek bir hash string yeterli
    public string PasswordHash { get; set; }

    // BACKWARD COMPATIBILITY: Eski sistemdeki kullanıcılar için
    // Migration sonrası bu alanlar null olabilir
    public byte[]? PasswordSalt { get; set; }
    public byte[]? PasswordHashLegacy { get; set; }

    public bool Status { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? EmailConfirmationToken { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<UserOperationClaim> UserOperationClaims { get; set; }

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        UserName = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        RefreshTokens = new HashSet<RefreshToken>();
        UserOperationClaims = new HashSet<UserOperationClaim>();
    }

    public User(
        string firstName,
        string lastName,
        string email,
        string userName,
        string passwordHash,
        bool status) : this()
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        PasswordHash = passwordHash;
        Status = status;
        CreatedDate = DateTime.UtcNow;
    }

    // BACKWARD COMPATIBILITY Constructor
    [Obsolete("Use constructor with passwordHash string instead")]
    public User(
        string firstName,
        string lastName,
        string email,
        string userName,
        byte[] passwordSalt,
        byte[] passwordHashLegacy,
        bool status) : this()
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        PasswordSalt = passwordSalt;
        PasswordHashLegacy = passwordHashLegacy;
        Status = status;
        CreatedDate = DateTime.UtcNow;
    }
}