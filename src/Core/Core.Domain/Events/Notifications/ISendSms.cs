namespace Notifications;

public interface ISendSms
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}
public class SendSms : ISendSms
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}