using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Shipment.Application.Shipments.Commands.ShipmentReceiveds;

namespace Shipment.Application.Consumer;

public class ShipmentReceivedConsumer : IConsumer<IShipmentReceived>
{
    private readonly Mediator _mediator;

    public ShipmentReceivedConsumer(Mediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IShipmentReceived> context)
    {
        var command = context.Message;

        await _mediator.Send(new ShipmentReceivedCommand
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState
        });
    }
}
public class ShipmentReceivedConsumerDefinition : ConsumerDefinition<ShipmentReceivedConsumer>
{
    public ShipmentReceivedConsumerDefinition()
    {
        ConcurrentMessageLimit = 3;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ShipmentReceivedConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}