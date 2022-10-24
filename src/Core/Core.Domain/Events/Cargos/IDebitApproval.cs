namespace Cargos;
public interface IDebitApproval //: IEvent
{
    public Guid CorrelationId { get; set; }
    public bool IsApproved { get; set; }
    public string CurrentState { get; set; }
}

public class DebitApproval : IDebitApproval
{
    public Guid CorrelationId { get; set; }
    public bool IsApproved { get; set; }
    public string CurrentState { get; set; }
}