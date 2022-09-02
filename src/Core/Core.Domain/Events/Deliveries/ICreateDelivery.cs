namespace Deliveries;

public interface ICreateDelivery
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class CreateDelivery : ICreateDelivery
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}