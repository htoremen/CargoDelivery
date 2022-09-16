namespace Saga.Application.Cargos;

public class CargoRejectedCommand : ICargoRejected
{
    public CargoRejectedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
