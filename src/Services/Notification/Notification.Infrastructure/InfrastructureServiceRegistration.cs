using BuildingBlocks.Messaging.SMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Services;
using Notification.Infrastructure.Contexts;
using Notification.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddNotificationInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("NotificationConnectionString")));

        services.AddScoped<INotificationLogRepository, NotificationLogRepository>();

        services.AddScoped<ISmsService, DebugSmsService>();

        return services;
    }
}
