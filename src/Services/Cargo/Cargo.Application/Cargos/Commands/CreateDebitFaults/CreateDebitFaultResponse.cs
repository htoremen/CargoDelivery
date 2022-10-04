namespace Cargo.Application.Cargos.Commands.CreateDebitFaults;

public class CreateDebitFaultResponse
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
