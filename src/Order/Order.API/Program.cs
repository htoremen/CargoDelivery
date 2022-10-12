using Core.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Order.API;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var appSettings = new AppSettings();
        builder.Configuration.Bind(appSettings);

        builder.Services.AddControllers( config =>
        {
           // config.Filters.Add(new OrderActionFilter());
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplicationServices(appSettings);
        builder.Services.AddInfrastructureServices();
        builder.Services.AddWebUIServices();
        builder.Services.AddEventBus(appSettings);
        builder.Services.AddHealthChecksServices(appSettings);

        //builder.Services.OpenTracingServices();
        // builder.Services.OpenTelemetryTranckingServices();

        //using var tracerProvider = Sdk.CreateTracerProviderBuilder()
        //        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Order.API"))
        //        .AddSource("Order.DistributedTracing")
        //        .AddConsoleExporter()
        //        .Build();

        builder.Services.AddOpenTelemetryTracingServices(appSettings);

        var app = builder.Build();
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
    }
}