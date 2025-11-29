using BuildingBlocks.CrossCutting.Exceptions.Extensions;
using Catalog.Infrastructure;
using Catalog.Application;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger (Klasik UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OpenAPI (JSON)
builder.Services.AddOpenApi();


builder.Services.AddCatalogApplicationServices();
builder.Services.AddCatalogInfrastructureServices(builder.Configuration);

var app = builder.Build();

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
app.Run();
