using Core.Infrastructure;
using Hangfire;
using Hangfire.SqlServer;
using HangfireJob.Service;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);
builder.Services.AddStaticValues(appSettings);
var Configuration = builder.Configuration;

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(Configuration.GetConnectionString("ConnectionString"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "alpha", "beta", "default" };
});

builder.Services.AddApplicationServices(appSettings);
builder.Services.AddInfrastructureServices();
builder.Services.AddWebUIServices();
builder.Services.AddEventBus(appSettings);
builder.Services.AddHealthChecksServices(appSettings);

var app = builder.Build();

//app.UseAuthorization();
//app.UseHealthChecks("/health", new HealthCheckOptions
//{
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});

app.UseHangfireDashboard();
app.Run();
