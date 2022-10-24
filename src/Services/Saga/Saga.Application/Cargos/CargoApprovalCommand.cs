namespace Saga.Application.Cargos;

public class CargoApprovalCommand : ICargoApproval
{
    public CargoApprovalCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public bool IsApproved { get; set; }
    public string CurrentState { get; set; }
}

