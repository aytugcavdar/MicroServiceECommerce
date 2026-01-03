using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Contexts;
using Order.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("OrderConnectionString")));

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
