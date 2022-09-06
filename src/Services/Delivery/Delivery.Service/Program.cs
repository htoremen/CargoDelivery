using Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices(appSettings);
builder.Services.AddInfrastructureServices();
builder.Services.AddWebUIServices();
builder.Services.AddEventBus(appSettings);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
