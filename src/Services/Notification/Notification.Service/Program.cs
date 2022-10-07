using Core.Infrastructure;
using Payment.Service;

var builder = WebApplication.CreateBuilder(args);
var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.MapControllers();

app.Run();
