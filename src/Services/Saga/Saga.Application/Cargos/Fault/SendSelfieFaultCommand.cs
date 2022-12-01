namespace Saga.Application.Cargos.Fault;

public class SendSelfieFaultCommand : ISendSelfieFault
{
    public SendSelfieFaultCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

