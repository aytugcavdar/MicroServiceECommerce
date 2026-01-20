using BuildingBlocks.CrossCutting.Exceptions.Extensions;
using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Contexts;
using BuildingBlocks.CrossCutting.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

// Swagger (Klasik UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OpenAPI (JSON)
builder.Services.AddOpenApi();


builder.Services.AddCatalogApplicationServices();
builder.Services.AddCatalogInfrastructureServices(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("CatalogConnectionString")!,
        name: "catalog-db",
        tags: new[] { "db", "postgresql" })
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "api" });
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<CatalogDbContext>();
        logger.LogInformation("CatalogDB migration'ları uygulanıyor...");

        // Veritabanı yoksa oluşturur, varsa bekleyen migrationları uygular
        context.Database.Migrate();

        logger.LogInformation("CatalogDB migration'ları başarıyla uygulandı.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "CatalogDB migration uygulanırken bir hata oluştu.");
    }
}

if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();

   
    app.MapOpenApi();
}

app.UseCorrelationId();

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
app.Run();
