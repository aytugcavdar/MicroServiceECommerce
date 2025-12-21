using BuildingBlocks.CrossCutting.Authentication;
using BuildingBlocks.CrossCutting.Exceptions.Extensions;
using BuildingBlocks.Infrastructure.Outbox;
using BuildingBlocks.Logging.Extensions;
using BuildingBlocks.Messaging;
using BuildingBlocks.Security;
using HealthChecks.NpgSql;
using Identity.Application;
using Identity.Domain.Events;
using Identity.Infrastructure;
using Identity.Infrastructure.Contexts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSerilogLogging("MicroECommerce.Identity");
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddIdentityApplicationServices();
builder.Services.AddIdentityInfrastructureServices(builder.Configuration);


builder.Services.AddHostedService<OutboxProcessor<IdentityDbContext>>();


builder.Services.AddHostedService<OutboxCleanupService<IdentityDbContext>>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSecurityServices(options =>
{
    options.SkipJwtRegistration = true;
    options.EnableEmailAuthentication = true;
    options.EnableOtpAuthentication = false; 
    options.EnableTwoFactorAuthentication = false; 
});
builder.Services.AddEmailServices(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("IdentityConnectionString")!,
        name: "identity-db",
        tags: new[] { "db", "postgresql" })
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "api" });

EventTypeRegistry.Register("UserCreated", typeof(UserCreatedDomainEvent));

var app = builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        
        dbContext.Database.Migrate();
        Console.WriteLine("✅ IdentityDB: Tablolar başarıyla oluşturuldu/güncellendi.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ IdentityDB Migration Hatası: {ex.Message}");
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
app.UseAuthentication();

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

Serilog.Log.Information("🚀 Identity.API is starting...");
app.Run();
Serilog.Log.Information("✅ Identity.API stopped");
