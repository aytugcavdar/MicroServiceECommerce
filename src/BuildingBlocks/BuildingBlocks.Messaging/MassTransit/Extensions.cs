using MassTransit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services,IConfiguration configuration, Assembly? assembly = null)
    { 
        var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>()
            ?? new RabbitMqOptions();

        services.AddMassTransit(busConfigurator =>
        {
            if (assembly != null)
            {
                busConfigurator.AddConsumers(assembly);
            }

            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });           
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;


    }
}
