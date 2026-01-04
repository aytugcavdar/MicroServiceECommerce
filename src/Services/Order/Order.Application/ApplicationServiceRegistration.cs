using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Features.Orders.Rules;
using System.Reflection;

namespace Order.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(assembly);

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<OrderBusinessRules>();

        return services;
    }
}
