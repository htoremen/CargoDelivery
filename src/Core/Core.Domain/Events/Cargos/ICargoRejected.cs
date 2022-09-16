namespace Cargos;
public interface ICargoRejected // : IEvent
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public class CargoRejected : ICargoRejected
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
    }
}