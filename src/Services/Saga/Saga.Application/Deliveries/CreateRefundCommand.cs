using Deliveries;

namespace Saga.Application.Deliveries;

public class CreateRefundCommand : ICreateRefund
{
    public CreateRefundCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}