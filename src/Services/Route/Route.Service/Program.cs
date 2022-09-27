using Core.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices(appSettings);
builder.Services.AddInfrastructureServices(appSettings);
builder.Services.AddGrpcServices(appSettings, builder.Configuration);
builder.Services.AddWebUIServices();
builder.Services.AddEventBus(appSettings);
builder.Services.AddHealthChecksServices(appSettings);

var app = builder.Build();
app.MapGrpcServices();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapControllers();
app.Run();
