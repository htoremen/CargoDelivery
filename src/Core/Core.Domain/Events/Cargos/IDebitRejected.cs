namespace Cargos;
public interface IDebitRejected // : IEvent
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public class DebitRejected : IDebitRejected
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
    }
}