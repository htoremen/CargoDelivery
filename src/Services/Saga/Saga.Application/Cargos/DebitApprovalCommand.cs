namespace Saga.Application.Cargos;

public class DebitApprovalCommand : IDebitApproval
{
    public DebitApprovalCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public bool IsApproved { get; set; }
    public string CurrentState { get; set; }
}

