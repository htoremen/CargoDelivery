namespace Saga.Application.Cargos;

public class CargoRejectedCommand : ICargoRejected
{
    public CargoRejectedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}
