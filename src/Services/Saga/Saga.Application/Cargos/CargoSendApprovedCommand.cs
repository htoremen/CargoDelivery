namespace Saga.Application.Cargos;

public class CargoSendApprovedCommand : ICargoSendApproved
{
    public CargoSendApprovedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
