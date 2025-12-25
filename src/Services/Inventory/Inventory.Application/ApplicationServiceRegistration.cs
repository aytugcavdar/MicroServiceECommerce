using BuildingBlocks.Messaging.MassTransit;
using Inventory.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddInventoryApplication(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var configuration = context.GetRequiredService<IConfiguration>();
                var rabbitMqOptions = configuration.GetSection("RabbitMqOptions").Get<RabbitMqOptions>();

                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });

                cfg.ReceiveEndpoint("stock-order-created-queue", e =>
                {
                    e.ConfigureConsumer<OrderCreatedConsumer>(context);
                });
            });
        });

        return services;
    }
}
