using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Services;
using Payment.Infrastructure.Contexts;
using Payment.Infrastructure.Gateways;
using Payment.Infrastructure.Repositories;

namespace Payment.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PaymentConnectionString")));

        // Repositories
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        // Gateways
        services.AddScoped<IPaymentGateway, MockPaymentGateway>();

        return services;
    }
}
