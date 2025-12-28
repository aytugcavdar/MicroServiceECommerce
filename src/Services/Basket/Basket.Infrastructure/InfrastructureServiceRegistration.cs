using Basket.API.Repositories;
using Basket.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddBasketInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionString");
        });

        services.AddScoped<IBasketRepository, BasketRepository>();

        return services;
    }
}
