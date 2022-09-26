using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Saga.Service;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddEventBus(builder.Configuration, appSettings);
builder.Services.AddHealthChecksServices(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

