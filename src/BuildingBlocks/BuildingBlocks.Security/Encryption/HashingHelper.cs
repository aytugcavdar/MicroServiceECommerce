using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace BuildingBlocks.Security.Encryption;

public static class HashingHelper
{
    /// <summary>
    /// BCrypt kullanarak şifre hash'i oluşturur
    /// workFactor: 12 (varsayılan, güvenlik seviyesi)
    /// </summary>
    public static string HashPassword(string password, int workFactor = 12)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
    }

    /// <summary>
    /// BCrypt kullanarak şifre doğrulama yapar
    /// </summary>
    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

}
