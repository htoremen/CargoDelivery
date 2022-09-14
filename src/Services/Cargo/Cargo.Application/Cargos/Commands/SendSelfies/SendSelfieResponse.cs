namespace Cargo.Application.Cargos.SendSelfie;

public class SendSelfieResponse
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
