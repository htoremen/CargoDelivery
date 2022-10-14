namespace Shipments;
public interface IShipmentReceived
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class ShipmentReceived : IShipmentReceived
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}