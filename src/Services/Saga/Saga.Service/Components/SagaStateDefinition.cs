
using Core.Infrastructure.MessageBrokers.RabbitMQ;

using MassTransit;
using MassTransit.Middleware;
using Saga.Domain.Instances;

namespace Saga.Service.Components;
public class SagaStateDefinition : SagaDefinition<CargoStateInstance>
{
    public SagaStateDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<CargoStateInstance> sagaConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}
