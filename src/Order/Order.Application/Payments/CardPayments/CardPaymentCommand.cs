using MassTransit;

namespace Order.Application.Payments.CardPayments;

public class CardPaymentCommand : IRequest<GenericResponse<CardPaymentResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class CardPaymentCommandHandler : IRequestHandler<CardPaymentCommand, GenericResponse<CardPaymentResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CardPaymentCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<CardPaymentResponse>> Handle(CardPaymentCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<ICardPayment>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<CardPaymentResponse>.Success(new CardPaymentResponse { CargoId = request.CargoId }, 200);

    }
}
