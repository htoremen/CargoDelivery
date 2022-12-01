namespace Events;

public interface ISendMail
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
public class SendMail : ISendMail
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CargoId { get; set; }
}