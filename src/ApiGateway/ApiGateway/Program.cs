using BuildingBlocks.Logging.Extensions;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. CONFIGURATION
// ============================================
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// ============================================
// 2. LOGGING (Serilog)
// ============================================
builder.AddSerilogLogging("ApiGateway");

// ============================================
// 3. YARP REVERSE PROXY
// ============================================
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ============================================
// 4. RATE LIMITING
// ============================================
builder.Services.AddRateLimiter(options =>
{
    // Fixed Window - Her dakika 100 istek
    options.AddFixedWindowLimiter("fixed-window", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10;
    });

    // Sliding Window - Daha hassas kontrol
    options.AddSlidingWindowLimiter("sliding-window", limiterOptions =>
    {
        limiterOptions.PermitLimit = 50;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.SegmentsPerWindow = 6; // Her 10 saniyede bir yenile
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });

    // Token Bucket - Burst traffic için
    options.AddTokenBucketLimiter("token-bucket", limiterOptions =>
    {
        limiterOptions.TokenLimit = 100;
        limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        limiterOptions.TokensPerPeriod = 20;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });

    // Rate limit aşıldığında dönen response
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

// ============================================
// 5. CORS
// ============================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    // Production için kısıtlı policy
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// ============================================
// 6. HEALTH CHECKS
// ============================================
builder.Services.AddHealthChecks();

var app = builder.Build();

// ============================================
// MIDDLEWARE PIPELINE
// ============================================

// 1. Serilog Request Logging
app.UseSerilogRequestLogging();

// 2. CORS
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
}
else
{
    app.UseCors("Production");
}

// 3. Rate Limiting
app.UseRateLimiter();

// 4. Health Check Endpoint
app.MapHealthChecks("/health");

// 5. Custom endpoints (Gateway bilgi endpoint'i)
app.MapGet("/", () => new
{
    service = "API Gateway",
    version = "1.0.0",
    status = "Running",
    timestamp = DateTime.UtcNow,
    endpoints = new
    {
        identity = "/identity/*",
        catalog = "/catalog/*",
        health = "/health",
        healthIdentity = "/health/identity",
        healthCatalog = "/health/catalog"
    }
});

// 6. YARP Reverse Proxy (En sonda olmalı)
app.MapReverseProxy();

Serilog.Log.Information("🚀 API Gateway (YARP) starting on http://localhost:5000...");
app.Run();
Serilog.Log.Information("✅ API Gateway stopped");