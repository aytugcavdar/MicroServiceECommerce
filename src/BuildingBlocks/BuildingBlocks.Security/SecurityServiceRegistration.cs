using BuildingBlocks.Security.EmailAuth;
using BuildingBlocks.Security.JWT;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Security;

public static class SecurityServiceRegistration
{
    /// <summary>
    /// Temel güvenlik servislerini ekler
    /// </summary>
    public static IServiceCollection AddSecurityServices(
        this IServiceCollection services,
        Action<SecurityOptions>? configure = null)
    {
        var options = new SecurityOptions();
        configure?.Invoke(options);

        // ============================================
        // 1. JWT Token Helper (Her zaman gerekli)
        // ============================================
        // NOT: ITokenHelper, AuthenticationExtensions'da da register edilir
        // Eğer AuthenticationExtensions kullanıyorsan, burayı çağırma
        if (!options.SkipJwtRegistration)
        {
            services.AddScoped<ITokenHelper, JwtHelper>();
        }

        // ============================================
        // 2. Email Authentication (Şu an aktif)
        // ============================================
        if (options.EnableEmailAuthentication)
        {
            services.AddScoped<IEmailAuthService, EmailAuthService>();
        }

        // ============================================
        // 3. OTP Authenticator (Gelecekte)
        // ============================================
        if (options.EnableOtpAuthentication)
        {
            // TODO: OTP servisi eklenecek
            // services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();
        }

        // ============================================
        // 4. Two-Factor Authentication (Gelecekte)
        // ============================================
        if (options.EnableTwoFactorAuthentication)
        {
            // TODO: 2FA servisi eklenecek
            // services.AddScoped<ITwoFactorService, TwoFactorService>();
        }

        return services;
    }
}

/// <summary>
/// Güvenlik özellikleri yapılandırması
/// </summary>
public class SecurityOptions
{
    /// <summary>
    /// ITokenHelper'ı register etme (AuthenticationExtensions kullanıyorsan true yap)
    /// </summary>
    public bool SkipJwtRegistration { get; set; } = false;

    /// <summary>
    /// Email authentication aktif mi?
    /// </summary>
    public bool EnableEmailAuthentication { get; set; } = true;

    /// <summary>
    /// OTP authentication aktif mi? (Google Authenticator)
    /// </summary>
    public bool EnableOtpAuthentication { get; set; } = false;

    /// <summary>
    /// Two-factor authentication aktif mi?
    /// </summary>
    public bool EnableTwoFactorAuthentication { get; set; } = false;
}