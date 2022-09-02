namespace Routes;

public interface IStartDelivery
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class StartDelivery : IStartDelivery
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}