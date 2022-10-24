using Cargos;
using Deliveries;
using MassTransit;
using MassTransit.Middleware;
using Saga.Domain.Instances;

namespace Saga.Service.Components;
public class SagaStateDefinition : SagaDefinition<CargoStateInstance>
{
    readonly IPartitioner _partition;

    public SagaStateDefinition()
    {
        _partition = new Partitioner(64, new Murmur3UnsafeHashGenerator());
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<CargoStateInstance> sagaConfigurator)
    {
        sagaConfigurator.Message<ICreateDebit>(x => x.UsePartitioner(_partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<ISendSelfie>(x => x.UsePartitioner(_partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<ICargoRejected>(x => x.UsePartitioner(_partition, m => m.Message.CorrelationId));
        sagaConfigurator.Message<IDebitApproval>(x => x.UsePartitioner(_partition, m => m.Message.CorrelationId));

        sagaConfigurator.Message<IShiftCompletion>(x => x.UsePartitioner(_partition, m => m.Message.CorrelationId));

        endpointConfigurator.UseMessageRetry(r => r.Intervals(20, 50, 100, 1000, 5000));
    }
}
