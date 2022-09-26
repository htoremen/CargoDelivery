using Core.Infrastructure;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Order.API;
using Order.Application.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(appSettings);
builder.Services.AddInfrastructureServices();
builder.Services.AddWebUIServices();
builder.Services.AddEventBus(appSettings);

builder.Services.AddHealthChecks()
    .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = registration => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(delegate (Options options)
{
    options.UIPath = "/hc-ui";
});

app.MapControllers();

app.Run();
