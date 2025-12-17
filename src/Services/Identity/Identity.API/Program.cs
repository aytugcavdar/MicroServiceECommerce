using BuildingBlocks.CrossCutting.Authentication;
using BuildingBlocks.CrossCutting.Exceptions.Extensions;
using BuildingBlocks.Infrastructure.Outbox;
using BuildingBlocks.Logging.Extensions;
using BuildingBlocks.Messaging;
using BuildingBlocks.Security;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Contexts;

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

var app = builder.Build();

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

Serilog.Log.Information("🚀 Identity.API is starting...");
app.Run();
Serilog.Log.Information("✅ Identity.API stopped");
