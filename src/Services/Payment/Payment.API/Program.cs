using BuildingBlocks.CrossCutting.Exceptions.Extensions;
using BuildingBlocks.Logging.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Payment.Application;
using Payment.Application.Consumers;
using Payment.Infrastructure;
using Payment.Infrastructure.Contexts;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.AddSerilogLogging("MicroECommerce.Payment");

// Controllers & API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Application & Infrastructure Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // Register ProcessPaymentCommandConsumer
    x.AddConsumer<ProcessPaymentCommandConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        // Configure endpoint for payment processing
        cfg.ReceiveEndpoint("payment-process", e =>
        {
            e.ConfigureConsumer<ProcessPaymentCommandConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

// OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: "Payment.API",
            serviceVersion: "1.0.0",
            serviceInstanceId: Environment.MachineName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
            options.Filter = (httpContext) =>
            {
                return !httpContext.Request.Path.StartsWithSegments("/health");
            };
        })
        .AddHttpClientInstrumentation(options =>
        {
            options.RecordException = true;
        })
        .AddSource("MassTransit")
        .AddConsoleExporter()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:4317");
        })
    );

// Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("PaymentConnectionString")!,
        name: "payment-db",
        tags: new[] { "db", "postgresql" })
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), 
        tags: new[] { "api" });

var app = builder.Build();

// Auto-migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("🔄 PaymentDB migration'ları uygulanıyor...");
        dbContext.Database.Migrate();
        logger.LogInformation("✅ PaymentDB migration'ları başarıyla uygulandı.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ PaymentDB migration uygulanırken bir hata oluştu.");
    }
}

app.UseSerilogRequestLogging();

// Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.ConfigureCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            service = "Payment.API",
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        });
        await context.Response.WriteAsync(result);
    }
});

Serilog.Log.Information("🚀 Payment.API starting...");
app.Run();
Serilog.Log.Information("✅ Payment.API stopped");
