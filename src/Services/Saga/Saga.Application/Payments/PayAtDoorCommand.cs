using Enums;

namespace Saga.Application.Payments;

public class PayAtDoorCommand : IPayAtDoor
{
    public PayAtDoorCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public PaymentType PaymentType { get; set; }
}
