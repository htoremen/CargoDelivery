namespace Notifications;

public interface IPushNotification
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}
public class PushNotification : IPushNotification
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}