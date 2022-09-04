namespace Order.Application.Payments.CardPayments;

public class CardPaymentResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}