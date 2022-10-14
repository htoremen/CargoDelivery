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
            DebitId = command.DebitId,
            CargoId = command.CargoId,
            CurrentState = command.CurrentState
        });
    }
}
