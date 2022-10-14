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
            CorrelationId = command.CorrelationId.ToString(),
            DebitId = command.DebitId.ToString(),
            CargoId = command.CargoId.ToString(),
            ShipmentTypeId = command.ShipmentTypeId,
            CurrentState = command.CurrentState
        });
    }
}
