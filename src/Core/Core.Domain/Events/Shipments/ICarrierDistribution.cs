namespace Shipments;

public interface ICarrierDistribution
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class CarrierDistribution : ICarrierDistribution
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}