namespace Cargos;

public interface ICreateCargo : IEvent
{
    public Guid CorrelationId { get; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime? SubmitDate { get; set; }
    public DateTime? AcceptDate { get; set; }
}

public class CreateCargo : ICreateCargo
{
    public Guid CorrelationId { get; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime? SubmitDate { get; set; }
    public DateTime? AcceptDate { get; set; }
}