using Cargo.Application.Cargos.CargoRejecteds;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Cargo.Application.Consumer;

public class CargoRejectedConsumer : IConsumer<ICargoRejected>
{
    private readonly IMediator _mediator;

    public CargoRejectedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICargoRejected> context)
    {
        var command = context.Message;

        await _mediator.Send(new CargoRejectedCommand
        {
            CorrelationId = command.CorrelationId,
        });

    }
}

public class CargoRejectedConsumerDefinition : ConsumerDefinition<CargoRejectedConsumer>
{
    public CargoRejectedConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CargoRejectedConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}