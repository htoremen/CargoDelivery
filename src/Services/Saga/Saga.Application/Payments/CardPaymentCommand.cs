namespace Saga.Application.Payments;

public class CardPaymentCommand : ICardPayment
{
    public CardPaymentCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

