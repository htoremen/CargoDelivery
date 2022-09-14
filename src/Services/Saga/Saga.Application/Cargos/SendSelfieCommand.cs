namespace Saga.Application.Cargos;

public class SendSelfieCommand : ISendSelfie
{
    public SendSelfieCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CargoId { get; set;}

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
