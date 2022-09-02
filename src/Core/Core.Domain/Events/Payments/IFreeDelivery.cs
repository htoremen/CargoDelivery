namespace Payments;

public interface IFreeDelivery
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class FreeDelivery : IFreeDelivery
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}