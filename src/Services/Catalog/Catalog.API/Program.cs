using BuildingBlocks.CrossCutting.Exceptions.Extensions;
using BuildingBlocks.CrossCutting.Middlewares;
using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Contexts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: "Catalog.API",
            serviceVersion: "1.0.0",
            serviceInstanceId: Environment.MachineName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
            options.EnrichWithHttpRequest = (activity, httpRequest) =>
            {
                activity.SetTag("http.request.size", httpRequest.ContentLength);
            };
            options.EnrichWithHttpResponse = (activity, httpResponse) =>
            {
                activity.SetTag("http.response.size", httpResponse.ContentLength);
            };
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
