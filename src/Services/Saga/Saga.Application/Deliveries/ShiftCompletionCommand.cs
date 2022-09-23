namespace Saga.Application.Deliveries;

public class ShiftCompletionCommand : IShiftCompletion
{
    public ShiftCompletionCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}