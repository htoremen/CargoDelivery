namespace Saga.Application.Deliveries;

public class NewDeliveryCommand : INewDelivery
{
    public NewDeliveryCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
