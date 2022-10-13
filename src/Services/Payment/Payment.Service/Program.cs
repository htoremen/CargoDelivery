using Core.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Payment.Service;

var builder = WebApplication.CreateBuilder(args);
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStaticValues(appSettings);

builder.Services.AddApplicationServices(appSettings);
builder.Services.AddInfrastructureServices(); 
builder.Services.AddWebUIServices();
builder.Services.AddEventBus(appSettings);
builder.Services.AddHealthChecksServices(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
