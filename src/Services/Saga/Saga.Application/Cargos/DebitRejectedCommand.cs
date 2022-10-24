namespace Saga.Application.Cargos;

public class DebitRejectedCommand : IDebitRejected
{
    public DebitRejectedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
