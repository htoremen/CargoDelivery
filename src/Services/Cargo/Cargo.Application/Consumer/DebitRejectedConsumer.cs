using Cargo.Application.Cargos.DebitRejecteds;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Cargo.Application.Consumer;

public class DebitRejectedConsumer : IConsumer<IDebitRejected>
{
    private readonly IMediator _mediator;

    public DebitRejectedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IDebitRejected> context)
    {
        var command = context.Message;

        await _mediator.Send(new DebitRejectedCommand
        {
            CorrelationId = command.CorrelationId,
        });

    }
}

public class DebitRejectedConsumerDefinition : ConsumerDefinition<DebitRejectedConsumer>
{
    public DebitRejectedConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<DebitRejectedConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}