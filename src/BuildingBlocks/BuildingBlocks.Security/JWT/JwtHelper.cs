using BuildingBlocks.Security.Encryption;
using BuildingBlocks.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BuildingBlocks.Security.JWT;

public class JwtHelper : ITokenHelper
{
    private readonly TokenOptions _tokenOptions;
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>()
            ?? throw new ArgumentNullException(nameof(TokenOptions));
    }

    public AccessToken CreateToken(Guid userId, string email, string userName, List<string> roles)
    {
        _accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);

        SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);

        JwtSecurityToken jwt = CreateJwtSecurityToken(
            _tokenOptions,
            userId,
            email,
            userName,
            roles,
            signingCredentials
        );

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string? token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken(token, _accessTokenExpiration);
    }

    public string CreateRefreshToken()
    {
        byte[] number = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(number);
        return Convert.ToBase64String(number);
    }

    private JwtSecurityToken CreateJwtSecurityToken(
        TokenOptions tokenOptions,
        Guid userId,
        string email,
        string userName,
        List<string> roles,
        SigningCredentials signingCredentials)
    {
        return new JwtSecurityToken(
            issuer: tokenOptions.Issuer,
            audience: tokenOptions.Audience,
            expires: _accessTokenExpiration,
            notBefore: DateTime.UtcNow,
            claims: SetClaims(userId, email, userName, roles),
            signingCredentials: signingCredentials
        );
    }

    private IEnumerable<Claim> SetClaims(Guid userId, string email, string userName, List<string> roles)
    {
        List<Claim> claims = new();

        claims.AddNameIdentifier(userId.ToString());
        claims.AddEmail(email);
        claims.AddName(userName);
        claims.AddRoles(roles.ToArray());

        return claims;
    }
}
