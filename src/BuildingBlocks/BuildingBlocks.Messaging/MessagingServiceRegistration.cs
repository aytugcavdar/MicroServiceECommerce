using BuildingBlocks.Messaging.Email;
using BuildingBlocks.Messaging.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Messaging;

public static class MessagingServiceRegistration
{
    /// <summary>
    /// Email servisi ve template engine'i ekler
    /// </summary>
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EmailOptions'ı configuration'dan yükle
        services.Configure<EmailOptions>(
            configuration.GetSection("EmailOptions")
        );

        // Template servisi
        services.AddScoped<ITemplateService, TemplateService>();

        // SMTP Email servisi
        services.AddScoped<IEmailService, SmtpEmailService>();

        return services;
    }

    /// <summary>
    /// Gelecekte SMS servisi eklenecek
    /// </summary>
    public static IServiceCollection AddSmsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: SMS servisi implementasyonu
        return services;
    }
}
