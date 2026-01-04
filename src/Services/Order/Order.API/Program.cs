using BuildingBlocks.CrossCutting.Exceptions.Extensions;
using BuildingBlocks.Logging.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Order.Application;
using Order.Application.Sagas;
using Order.Domain.Entities;
using Order.Infrastructure;
using Order.Infrastructure.Contexts;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);


builder.AddSerilogLogging("MicroECommerce.Order");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);


builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderStateMachine, OrderSagaState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.ExistingDbContext<OrderDbContext>();
            r.UsePostgres();
        });

    // RabbitMQ Konfigürasyonu
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        // Saga için endpoint
        cfg.ReceiveEndpoint("order-state-machine", e =>
        {
            e.ConfigureSaga<OrderSagaState>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("🔄 OrderDB migration'ları uygulanıyor...");
        dbContext.Database.Migrate();
        logger.LogInformation("✅ OrderDB migration'ları başarıyla uygulandı.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ OrderDB migration uygulanırken bir hata oluştu.");
    }
}

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
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

Serilog.Log.Information("🚀 Order.API starting...");
app.Run();
Serilog.Log.Information("✅ Order.API stopped");