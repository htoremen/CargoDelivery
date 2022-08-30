namespace Saga.Application.Cargos;

public class CargoApprovedCommand : ICargoApproved
{
    public CargoApprovedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

