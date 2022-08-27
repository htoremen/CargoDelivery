namespace Core.Domain.Events.Cargos;
public interface ICargoApproved : IEvent
{
    public Guid OrderId { get; set; }
}

public class CargoApproved : ICargoApproved
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
}