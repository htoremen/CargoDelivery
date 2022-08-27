namespace Core.Domain.Events.Cargos;

public interface ICreateCargo : IEvent
{
    public Guid CargoId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}

public class CreateCargo : ICreateCargo
{
    public Guid Id { get; set; }
    public Guid CargoId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}
