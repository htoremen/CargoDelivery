

namespace Saga.Application.Shipments;

public class ShipmentReceivedCommand : IShipmentReceived
{
    public ShipmentReceivedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

