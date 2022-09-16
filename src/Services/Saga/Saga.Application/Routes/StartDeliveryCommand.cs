namespace Saga.Application.Routes;

public class StartDeliveryCommand : IStartDelivery
{
    public StartDeliveryCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
