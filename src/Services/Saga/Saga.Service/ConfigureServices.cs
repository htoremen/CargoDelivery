﻿using Confluent.Kafka;
using Core.Application;
using Core.Domain;
using Core.Domain.Bus;
using Core.Domain.Enums;
using Core.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Domain.Instances;
using Saga.Infrastructure.Persistence;
using Saga.Service.StateMachines;

namespace Saga.Service;
public static class ConfigureServices
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationRoot configuration, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);

        var config = appSettings.MessageBroker.RabbitMQ;

        services.AddMassTransit<IEventBus>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddBus(factory => MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(config.HostName, config.VirtualHost, h =>
                {
                    h.Username(config.UserName);
                    h.Password(config.Password);
                });

                cfg.UseJsonSerializer();
                cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));

                cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoSaga], e =>
                {
                    e.ConfigureSaga<CargoStateInstance>(factory);
                });
            }));

            x.AddSagaStateMachine<CargoStateMachine, CargoStateInstance>()
                .EntityFrameworkRepository(config =>
                {
                    config.AddDbContext<DbContext, CargoStateDbContext>((p, b) =>
                    {
                        b.UseSqlServer(configuration.GetConnectionString("CargoStateDb"));
                    });
                });
        });

        //services.AddSingleton(rabbitMQConfig);
        //services.AddTransient(typeof(IEventBusService<>), typeof(EventBusService<>));
        //services.AddTransient(typeof(IEventBusManager<>), typeof(EventBusManager<>));

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });
        return services;
    }
}