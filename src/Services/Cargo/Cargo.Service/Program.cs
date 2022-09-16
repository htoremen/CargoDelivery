using Cargo.GRPC.Services;
using Core.Infrastructure;
using NoSQLMongo.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices(appSettings, builder.Configuration);
builder.Services.AddGrpcServices(appSettings, builder.Configuration);
builder.Services.AddInfrastructureServices(appSettings);
builder.Services.AddWebUIServices();
builder.Services.AddEventBus(appSettings);

var app = builder.Build();

app.MapGrpcServices();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
