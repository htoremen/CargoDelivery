namespace Cargos;
public interface ISendSelfie //: IEvent
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public string Selfie { get; set; }
}

public class SendSelfie : ISendSelfie
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public string Selfie { get; set; }
}