namespace Cargos;

public interface ICreateCargo //: IEvent
{
    public Guid CorrelationId { get; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
}

public class CreateCargo : ICreateCargo
{
    public Guid CorrelationId { get; private set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
}