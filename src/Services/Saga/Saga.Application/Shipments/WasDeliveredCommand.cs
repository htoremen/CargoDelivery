namespace Saga.Application.Shipments;

public class WasDeliveredCommand : IWasDelivered
{
    public WasDeliveredCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}
