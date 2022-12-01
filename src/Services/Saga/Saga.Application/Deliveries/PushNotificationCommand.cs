

namespace Saga.Application.Deliveries;

public class PushNotificationCommand : IPushNotification
{
    public PushNotificationCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
