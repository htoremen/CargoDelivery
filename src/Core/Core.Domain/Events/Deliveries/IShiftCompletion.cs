namespace Events;

public interface IShiftCompletion
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class ShiftCompletion : IShiftCompletion
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}