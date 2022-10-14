using Shipments;

namespace Saga.Application.Shipments;

public class ShipmentReceivedCommand : IShipmentReceived
{
    public ShipmentReceivedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CargoId { get; set; }
    public int ShipmentTypeId { get; set; }
    public string CurrentState { get; set; }
}

