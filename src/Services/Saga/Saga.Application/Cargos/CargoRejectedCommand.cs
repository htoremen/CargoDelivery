namespace Saga.Application.Cargos;

public class CargoRejectedCommand : ICargoRejected
{
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; }
}
