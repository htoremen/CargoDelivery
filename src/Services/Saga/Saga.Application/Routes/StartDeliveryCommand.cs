namespace Saga.Application.Routes;

public class StartDeliveryCommand : IStartDelivery
{
    public StartDeliveryCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}
