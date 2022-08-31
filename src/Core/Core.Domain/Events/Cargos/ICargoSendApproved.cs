namespace Cargos;

public interface ICargoSendApproved //: IEvent
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CargoSendApproved : ICargoSendApproved
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}