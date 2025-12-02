using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public class User : Entity<Guid>, IAggregateRoot
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; } // IdentityUser'da vardı, manuel ekledik
    public string Email { get; set; }    // IdentityUser'da vardı, manuel ekledik

    // Şifreleme için (IdentityUser bunu kendi içinde yapar ama biz manuel yapıyoruz)
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }

    public bool Status { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? EmailConfirmationToken { get; set; }

    // --- REFRESH TOKEN MANTIK DEĞİŞİKLİĞİ ---
    // Senin kodunda RefreshToken tek bir stringdi.
    // Doğrusu: Bir kullanıcının hem telefondan hem bilgisayardan girmesi için
    // RefreshToken'ın bir LİSTE olması gerekir.
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<UserOperationClaim> UserOperationClaims { get; set; }

    public User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        RefreshTokens = new HashSet<RefreshToken>();
        UserOperationClaims = new HashSet<UserOperationClaim>();
    }

    public User(string firstName, string lastName, string email, string userName, byte[] passwordSalt, byte[] passwordHash, bool status) : this()
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        Status = status;
        CreatedDate = DateTime.UtcNow;
    }
}
