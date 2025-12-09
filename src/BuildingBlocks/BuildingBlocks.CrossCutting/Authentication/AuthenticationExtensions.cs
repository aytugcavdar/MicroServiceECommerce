using BuildingBlocks.Security.Encryption;
using BuildingBlocks.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace BuildingBlocks.CrossCutting.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var tokenOptions = configuration
            .GetSection("TokenOptions")
            .Get<TokenOptions>();

        if (tokenOptions == null)
        {
            throw new InvalidOperationException(
                "TokenOptions configuration is missing in appsettings.json. " +
                "Please add 'TokenOptions' section with Audience, Issuer, SecurityKey, etc."
            );
        }
        if (string.IsNullOrEmpty(tokenOptions.SecurityKey) || tokenOptions.SecurityKey.Length < 32)
        {
            throw new InvalidOperationException(
                "TokenOptions.SecurityKey must be at least 32 characters long for security reasons."
            );
        }
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = tokenOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = tokenOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),

                NameClaimType = System.Security.Claims.ClaimTypes.Name,
                RoleClaimType = System.Security.Claims.ClaimTypes.Role
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }

                    return Task.CompletedTask;
                },

                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "You are not authorized to access this resource. Please login.",
                        statusCode = 401
                    });

                    return context.Response.WriteAsync(result);
                },

                // 403 Forbidden response özelleştirmesi
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "You do not have permission to access this resource. Required role/claim is missing.",
                        statusCode = 403
                    });

                    return context.Response.WriteAsync(result);
                },

                OnTokenValidated = context =>
                {
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization(options =>
        {
            // Default policy: Sadece authenticate olmuş kullanıcılar
            options.FallbackPolicy = null; // Her endpoint için [Authorize] gerekli değil

            // Custom policy: Admin rolü gerekli
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            // Custom policy: User veya Admin rolü
            options.AddPolicy("UserOrAdmin", policy =>
                policy.RequireRole("User", "Admin"));

            // Custom policy: Specific claim gerekli
            options.AddPolicy("CanManageProducts", policy =>
                policy.RequireClaim("permission", "Product.Manage"));
        });

        services.AddScoped<ITokenHelper, JwtHelper>();

        return services;
    }
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // JWT Security Definition
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
            });

            // Security Requirement
            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
