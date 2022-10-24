namespace Cargos;
public interface ICargoApproval //: IEvent
{
    public Guid CorrelationId { get; set; }
    public bool IsApproved { get; set; }
    public string CurrentState { get; set; }
}

public class CargoApproval : ICargoApproval
{
    public Guid CorrelationId { get; set; }
    public bool IsApproved { get; set; }
    public string CurrentState { get; set; }
}