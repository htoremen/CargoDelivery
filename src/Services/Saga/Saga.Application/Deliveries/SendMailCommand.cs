

namespace Saga.Application.Deliveries;

public class SendMailCommand : ISendMail
{
    public SendMailCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
