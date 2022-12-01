using Enums;

namespace Events;

public interface ICreateDelivery
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public PaymentType PaymentType { get; set; }
}
public class CreateDelivery : ICreateDelivery
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CargoId { get; set; }
    public PaymentType PaymentType { get; set; }
}