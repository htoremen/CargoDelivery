namespace Saga.Application.Cargos;

public class CreateCargoCommand : ICreateCargo
{
    public CreateCargoCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; private set; }

    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime? SubmitDate { get; set; }
    public DateTime? AcceptDate { get; set; }
}