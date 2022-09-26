using Core.Infrastructure;
using Order.API;
using Order.Application.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddHealthChecks()
    .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings));

//builder.Services.AddHealthChecksUI(opt =>
//{
//    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
//    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
//    opt.SetApiMaxActiveRequests(1); //api requests concurrency

//    opt.AddHealthCheckEndpoint("Order-Api", "https://localhost:44316/health"); //map health check api

//}).AddSqlServerStorage(appSettings.ConnectionStrings.Monitoring);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(appSettings);
builder.Services.AddInfrastructureServices();
builder.Services.AddWebUIServices();
builder.Services.AddEventBus(appSettings);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHealthChecks("/health");

//app.UseHealthChecks("/health-ui", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});
app.MapControllers();

app.Run();
