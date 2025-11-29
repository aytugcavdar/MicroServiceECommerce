using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Catalog.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddCatalogApplicationServices(this IServiceCollection services)
    {
        // "Executing Assembly" demek, bu kodun çalıştığı yer (yani Application katmanı) demektir.
        var assembly = Assembly.GetExecutingAssembly();

        // 1. AutoMapper'ı bu katmanda bulduğu bütün Profile dosyalarına göre ayarlar
        services.AddAutoMapper(assembly);

        // 2. MediatR'ı bu katmanda bulduğu bütün Command/Query Handler'lara göre ayarlar
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        return services;
    }
}
