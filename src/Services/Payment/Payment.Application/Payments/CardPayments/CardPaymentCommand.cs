
using Deliveries;
using MassTransit;

namespace Payment.Application.Payments.CardPayments;

public class CardPaymentCommand : IRequest<GenericResponse<CardPaymentResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CardPaymentCommandHandler : IRequestHandler<CardPaymentCommand, GenericResponse<CardPaymentResponse>>
{
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public CardPaymentCommandHandler(IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _deliveryCompleted = deliveryCompleted;
    }

    public async Task<GenericResponse<CardPaymentResponse>> Handle(CardPaymentCommand request, CancellationToken cancellationToken)
    {
        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId,


        }, null, cancellationToken);
        return GenericResponse<CardPaymentResponse>.Success(new CardPaymentResponse { }, 200);

    }
}
