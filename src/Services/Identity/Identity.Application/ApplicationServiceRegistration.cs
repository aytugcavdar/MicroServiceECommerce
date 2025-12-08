using BuildingBlocks.CrossCutting.Validation;
using BuildingBlocks.Security.JWT;
using FluentValidation;
using Identity.Application.Features.Auth.ConfirmEmail.Rules;
using Identity.Application.Features.Auth.Login.Rules;
using Identity.Application.Features.Auth.Register.Rules;
using Identity.Application.Features.Users.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace Identity.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddIdentityApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // ============================================
        // 1. AUTOMAPPER
        // ============================================
        services.AddAutoMapper(assembly);

        // ============================================
        // 2. FLUENTVALIDATION
        // ============================================
        services.AddValidatorsFromAssembly(assembly);

        // ============================================
        // 3. MEDIATR
        // ============================================
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // ============================================
        // 4. BUSINESS RULES
        // ============================================
        // Scoped olarak kaydet (her request'te yeni instance)
        services.AddScoped<AuthBusinessRules>();


        return services;
    }
}
