using BuildingBlocks.Messaging.Email;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.Messaging.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace BuildingBlocks.Messaging;

public static class MessagingServiceRegistration
{
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly? assembly = null)
    {
        // EmailOptions'ı configuration'dan yükle
        services.Configure<EmailOptions>(
            configuration.GetSection("EmailOptions")
        );

        // Template servisi
        services.AddScoped<ITemplateService, TemplateService>();

        // SMTP Email servisi
        services.AddScoped<IEmailService, SmtpEmailService>();

        //RabbitMQ / MassTransit
        services.AddMessageBus(configuration, assembly);

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
