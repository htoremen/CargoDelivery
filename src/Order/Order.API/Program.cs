using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using Order.API;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var appSettings = new AppSettings();
        builder.Configuration.Bind(appSettings);

        builder.Services.AddHealthChecks()
            .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplicationServices(appSettings);
        builder.Services.AddInfrastructureServices();
        builder.Services.AddWebUIServices();
        builder.Services.AddEventBus(appSettings);


        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseHealthChecks("/health");

        app.MapControllers();

        app.Run();
    }
}