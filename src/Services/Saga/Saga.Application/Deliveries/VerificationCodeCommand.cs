namespace Saga.Application.Deliveries;

public class VerificationCodeCommand : IVerificationCode
{
    public VerificationCodeCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int Code { get; set; }
}
