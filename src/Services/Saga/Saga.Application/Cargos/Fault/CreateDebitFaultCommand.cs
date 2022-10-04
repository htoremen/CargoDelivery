namespace Saga.Application.Cargos;

public class CreateDebitFaultCommand : Fault<ICreateDebit>
{
    public CreateDebitFaultCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }

    public ExceptionInfo[] Exceptions { get; set; }

    public ICreateDebit Message => throw new NotImplementedException();

    public Guid FaultId => throw new NotImplementedException();

    public Guid? FaultedMessageId => throw new NotImplementedException();

    public DateTime Timestamp => throw new NotImplementedException();

    public HostInfo Host => throw new NotImplementedException();

    public string[] FaultMessageTypes => throw new NotImplementedException();
}

