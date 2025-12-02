using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public class RefreshToken : Entity<Guid>
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }

    // Navigation property
    public User User { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    public RefreshToken()
    {
        Token = string.Empty;
        CreatedByIp = string.Empty;
    }

    public RefreshToken(Guid userId, string token, DateTime expiresAt, string createdByIp)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedByIp = createdByIp;
        CreatedDate = DateTime.UtcNow;
    }

    public void Revoke(string ip, string reason, string? replacedByToken = null)
    {
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = ip;
        ReasonRevoked = reason;
        ReplacedByToken = replacedByToken;
    }
}
