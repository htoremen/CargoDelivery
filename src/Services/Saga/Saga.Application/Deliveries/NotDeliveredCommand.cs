using Deliveries;

namespace Saga.Application.Deliveries;

public class NotDeliveredCommand : INotDelivered
{
    public NotDeliveredCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
