using Inventory.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

                // HATA BURADAYDI: RabbitMqOptions sınıfı yerine doğrudan okuyoruz.
                // Eğer config gelmezse varsayılan değerleri ("rabbitmq", "guest") kullanır.
                var host = configuration["RabbitMQ:Host"] ?? "rabbitmq";
                var userName = configuration["RabbitMQ:UserName"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(host, "/", h =>
                {
                    h.Username(userName);
                    h.Password(password);
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