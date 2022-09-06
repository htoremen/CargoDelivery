using Enums;

namespace Payments;

public interface ICardPayment
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public PaymentType PaymentType { get; set; }
}

public class CardPayment : ICardPayment
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public PaymentType PaymentType { get; set; }
}