namespace Saga.Application.Cargos;

public class CreateCargoCommand : ICreateCargo
{
    public CreateCargoCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; }

    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
}