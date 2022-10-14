namespace Shipments;
public interface IShipmentReceived
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CargoId { get; set; }
    public int ShipmentTypeId { get; set; }
    public string CurrentState { get; set; }
}

public class ShipmentReceived : IShipmentReceived
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CargoId { get; set; }
    public int ShipmentTypeId { get; set; }
    public string CurrentState { get; set; }
}