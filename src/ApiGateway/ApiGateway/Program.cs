using BuildingBlocks.CrossCutting.Authentication;
using BuildingBlocks.Logging.Extensions;
using Microsoft.AspNetCore.RateLimiting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.AddSerilogLogging("ApiGateway");

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: "ApiGateway",
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

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed-window", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });

    options.AddSlidingWindowLimiter("sliding-window", limiterOptions =>
    {
        limiterOptions.PermitLimit = 50;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.SegmentsPerWindow = 6;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });

    options.AddTokenBucketLimiter("token-bucket", limiterOptions =>
    {
        limiterOptions.TokenLimit = 100;
        limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        limiterOptions.TokensPerPeriod = 20;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            success = false,
            message = "Too many requests. Please try again later.",
            retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                ? retryAfter.ToString()
                : "60 seconds",
            statusCode = 429
        }, cancellationToken);
    };
});

// ✅ CORS - Frontend için özel yapılandırma
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("X-Correlation-ID", "X-Request-ID");
    });

    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("X-Correlation-ID", "X-Request-ID");
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();

// ✅ CORS - Environment'a göre
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowFrontend");
}
else
{
    app.UseCors("Production");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapHealthChecks("/health");

// ✅ API Info Endpoint
app.MapGet("/", () => new
{
    service = "MicroECommerce API Gateway",
    version = "1.0.0",
    status = "Running",
    timestamp = DateTime.UtcNow,
    endpoints = new
    {
        auth = new
        {
            register = "POST /api/identity/auth",
            login = "POST /api/identity/auth/login",
            users = "GET /api/identity/users"
        },
        catalog = new
        {
            products = "GET /api/catalog/product",
            categories = "GET /api/catalog/category",
            categoryTree = "GET /api/catalog/category/tree"
        },
        basket = new
        {
            items = "GET/POST/PUT/DELETE /api/basket/*"
        },
        order = new
        {
            orders = "GET/POST /api/order/*"
        },
        health = new
        {
            overall = "GET /health",
            catalog = "GET /health/catalog",
            basket = "GET /health/basket",
            order = "GET /health/order",
            identity = "GET /health/identity"
        }
    }
}).WithTags("Info");

app.MapReverseProxy();

Serilog.Log.Information("🚀 API Gateway (YARP) starting on port 5000...");
app.Run();
Serilog.Log.Information("✅ API Gateway stopped");