using Core.Infrastructure;
using Saga.Service;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddEventBus(builder.Configuration, appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();

