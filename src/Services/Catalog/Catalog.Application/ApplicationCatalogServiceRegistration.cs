using BuildingBlocks.CrossCutting.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Catalog.Application;

public static class ApplicationCatalogServiceRegistration
{
    public static IServiceCollection AddCatalogApplicationServices(this IServiceCollection services)
    {
        // "Executing Assembly" demek, bu kodun çalıştığı yer (yani Application katmanı) demektir.
        var assembly = Assembly.GetExecutingAssembly();

        // 1. AutoMapper'ı bu katmanda bulduğu bütün Profile dosyalarına göre ayarlar
        services.AddAutoMapper(assembly);

        // 2. FluentValidation - Tüm Validator'ları otomatik olarak bul ve ekle
        services.AddValidatorsFromAssembly(assembly);


        // 3. MediatR'ı bu katmanda bulduğu bütün Command/Query Handler'lara göre ayarlar
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);

            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
