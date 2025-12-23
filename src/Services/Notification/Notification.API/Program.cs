using BuildingBlocks.Logging.Extensions;
using BuildingBlocks.Messaging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Notification.Application;
using Notification.Infrastructure;
using Notification.Infrastructure.Contexts;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddSerilogLogging("MicroECommerce.Notification");



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNotificationApplicationServices();
builder.Services.AddNotificationInfrastructureServices(builder.Configuration);

builder.Services.AddEmailServices(
    builder.Configuration,
    Assembly.GetExecutingAssembly());

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("NotificationConnectionString")!,
        name: "notification-db",
        tags: new[] { "db", "postgresql" })
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("✅ NotificationDB: Migrations applied successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ NotificationDB Migration Error: {ex.Message}");
    }
}

app.UseSerilogRequestLogging();

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
                duration = e.Value.Duration.TotalMilliseconds
            })
        });
        await context.Response.WriteAsync(result);
    }
});

Serilog.Log.Information("🚀 Notification.API starting...");
app.Run();
Serilog.Log.Information("✅ Notification.API stopped");