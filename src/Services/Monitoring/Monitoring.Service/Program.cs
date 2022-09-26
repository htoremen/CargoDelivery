using Core.Infrastructure;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);


var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.Services.AddHealthChecksUI(opt =>
{
    //opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
    //opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
    //opt.SetApiMaxActiveRequests(1); //api requests concurrency

    //opt.AddHealthCheckEndpoint("Order-Api", "https://localhost:44316/health");
    //opt.AddHealthCheckEndpoint("Saga-Service", "https://localhost:5010/health");

}).AddSqlServerStorage(appSettings.ConnectionStrings.Monitoring);


var app = builder.Build();

app.UseHealthChecksUI(options => options.UIPath = "/health-ui");

app.Run();
