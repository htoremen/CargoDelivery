namespace Saga.Application.Payments;

public class FreeDeliveryCommand : IFreeDelivery
{
    public FreeDeliveryCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}
