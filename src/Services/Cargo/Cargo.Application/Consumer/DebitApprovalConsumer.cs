using Cargo.Application.Cargos.DebitApprovals;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Cargo.Application.Consumer;

public class DebitApprovalConsumer : IConsumer<IDebitApproval>
{
    private readonly IMediator _mediator;

    public DebitApprovalConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IDebitApproval> context)
    {
        var command = context.Message;

        await _mediator.Send(new DebitApprovalCommand
        {
            CorrelationId = command.CorrelationId,
            IsApproved = command.IsApproved,
            CurrentState = command.CurrentState
        });
    }
}

public class DebitApprovalConsumerDefinition : ConsumerDefinition<DebitApprovalConsumer>
{
    public DebitApprovalConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<DebitApprovalConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}