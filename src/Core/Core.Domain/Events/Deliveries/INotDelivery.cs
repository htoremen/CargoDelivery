namespace Deliveries;

public interface INotDelivery
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class NotDelivery : INotDelivery
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}