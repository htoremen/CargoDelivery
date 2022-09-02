namespace Deliveries;

public interface ICreateRefund
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class CreateRefund : ICreateRefund
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}