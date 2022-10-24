using Cargo.Application.Cargos.CargoApprovals;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Cargo.Application.Consumer;

public class CargoApprovalConsumer : IConsumer<ICargoApproval>
{
    private readonly IMediator _mediator;

    public CargoApprovalConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICargoApproval> context)
    {
        var command = context.Message;

        await _mediator.Send(new CargoApprovalCommand
        {
            CorrelationId = command.CorrelationId,
            IsApproved = command.IsApproved,
            CurrentState = command.CurrentState
        });
    }
}

public class CargoApprovalConsumerDefinition : ConsumerDefinition<CargoApprovalConsumer>
{
    public CargoApprovalConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CargoApprovalConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}