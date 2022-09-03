namespace Order.Application.Deliveries.ShiftCompletions;

public class ShiftCompletionResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
