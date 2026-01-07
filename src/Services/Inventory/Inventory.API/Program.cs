using BuildingBlocks.Logging.Extensions;
using Inventory.Application;
using Inventory.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 


builder.Services.AddOpenApi();
builder.AddSerilogLogging("MicroECommerce.Inventory");
builder.Services.AddInventoryInfrastructure(builder.Configuration);
builder.Services.AddInventoryApplication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
