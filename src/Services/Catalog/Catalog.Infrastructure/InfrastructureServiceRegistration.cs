using BuildingBlocks.Core.Repositories;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Contexts;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddCatalogInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CatalogConnectionString")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
