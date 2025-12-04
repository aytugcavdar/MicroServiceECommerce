using BuildingBlocks.Logging.Configurations;
using BuildingBlocks.Logging.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Logging.Extensions;

public static class LoggingExtensions
{
    /// <summary>
    /// Serilog'u ASP.NET Core'a ekler
    /// Kullanım: builder.AddSerilogLogging("Catalog.API");
    /// </summary>
    public static WebApplicationBuilder AddSerilogLogging(
        this WebApplicationBuilder builder,
        string applicationName)
    {
        // HttpContextAccessor ekle (CorrelationId için gerekli)
        builder.Services.AddHttpContextAccessor();

        // CorrelationIdEnricher'ı ekle
        builder.Services.AddSingleton<CorrelationIdEnricher>();

        // Bootstrap logger oluştur (Uygulama ayağa kalkarken hata olursa yakalamak için)
        Log.Logger = SerilogConfiguration.CreateBootstrapLogger(applicationName);

        Log.Information("🚀 Starting {ApplicationName}...", applicationName);

        try
        {
            // Serilog'u host'a ekle
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                // Ortama göre logger seç
                if (context.HostingEnvironment.IsDevelopment())
                {
                    // Development logger ayarları
                    configuration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("Application", applicationName)
                        .Enrich.WithProperty("Environment", "Development")
                        .Enrich.With(services.GetRequiredService<CorrelationIdEnricher>()) // Correlation ID ekle
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                        .WriteTo.File($"Logs/{applicationName}-dev-.txt", rollingInterval: RollingInterval.Day);
                }
                else
                {
                    // Production logger ayarları (Seq, JSON format vs.)
                    var seqUrl = context.Configuration["Serilog:WriteTo:1:Args:serverUrl"] ?? "http://localhost:5341";

                    configuration
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("Application", applicationName)
                        .Enrich.WithProperty("Environment", "Production")
                        .Enrich.With(services.GetRequiredService<CorrelationIdEnricher>()) // Correlation ID ekle
                        .WriteTo.Console()
                        .WriteTo.Seq(seqUrl);
                }
            });
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }

        Log.Information("✅ Serilog configured successfully");

        return builder;
    }

    /// <summary>
    /// Serilog Request Logging middleware ekler
    /// Her HTTP isteğini otomatik loglar
    /// </summary>
    public static WebApplication UseSerilogRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            // Log template
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

            // Ekstra bilgiler ekle
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress?.ToString());

                // Query string varsa ekle
                if (httpContext.Request.QueryString.HasValue)
                {
                    diagnosticContext.Set("QueryString", httpContext.Request.QueryString.Value);
                }

                // Response size
                if (httpContext.Response.ContentLength.HasValue)
                {
                    diagnosticContext.Set("ResponseSize", httpContext.Response.ContentLength.Value);
                }

                // User ID (authenticated ise)
                if (httpContext.User.Identity?.IsAuthenticated == true)
                {
                    var userId = httpContext.User.FindFirst("sub")?.Value
                              ?? httpContext.User.FindFirst("nameidentifier")?.Value;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        diagnosticContext.Set("UserId", userId);
                    }
                }
            };

            // Hangi seviyelerde loglansın?
            options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex != null || httpContext.Response.StatusCode >= 500)
                    return Serilog.Events.LogEventLevel.Error;

                if (httpContext.Response.StatusCode >= 400)
                    return Serilog.Events.LogEventLevel.Warning;

                if (elapsed > 1000) // 1 saniyeden uzun süren istekler Warning olsun
                    return Serilog.Events.LogEventLevel.Warning;

                return Serilog.Events.LogEventLevel.Information;
            };
        });

        return app;
    }
}
