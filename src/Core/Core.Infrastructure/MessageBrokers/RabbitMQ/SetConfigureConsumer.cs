using MassTransit;

namespace Core.Infrastructure.MessageBrokers.RabbitMQ;

public static class SetConfigureConsumer
{
    public static IReceiveEndpointConfigurator SetConfigure(this IReceiveEndpointConfigurator endpointConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
        endpointConfigurator.PrefetchCount = 1;
        endpointConfigurator.UseCircuitBreaker(cb =>
        {
            cb.TrackingPeriod = TimeSpan.FromMinutes(RabbitMQStaticValues.TrackingPeriod);
            cb.TripThreshold = RabbitMQStaticValues.TripThreshold;
            cb.ActiveThreshold = RabbitMQStaticValues.ActiveThreshold;
            cb.ResetInterval = TimeSpan.FromMinutes(RabbitMQStaticValues.ResetInterval);
        });

        return endpointConfigurator;
    }

    public static int ConcurrentMessageLimit()
    {
        return 3;
    }
}
