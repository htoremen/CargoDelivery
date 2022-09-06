
using Enums;
using Payments;

namespace Delivery.Application.Deliveries.CreateDeliveries;

public class CreateDeliveryCommand : IRequest<GenericResponse<CreateDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public PaymentType PaymentType { get; set; }
}
public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, GenericResponse<CreateDeliveryResponse>>
{
    private readonly IMessageSender<ICardPayment> _cardPayment;
    private readonly IMessageSender<IPayAtDoor> _payAtDoor;
    private readonly IMessageSender<IFreeDelivery> _freeDelivery;

    public CreateDeliveryCommandHandler(IMessageSender<ICardPayment> cardPayment, IMessageSender<IPayAtDoor> payAtDoor, IMessageSender<IFreeDelivery> freeDelivery)
    {
        _cardPayment = cardPayment;
        _payAtDoor = payAtDoor;
        _freeDelivery = freeDelivery;
    }

    public async Task<GenericResponse<CreateDeliveryResponse>> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        if (request.PaymentType == PaymentType.CardPayment)
        {
            await _cardPayment.SendAsync(new CardPayment
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId,
                PaymentType = request.PaymentType
            }, null, cancellationToken);
        }
        else if (request.PaymentType == PaymentType.PayAtDoor)
        {
            await _payAtDoor.SendAsync(new PayAtDoor
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId,
                PaymentType = request.PaymentType
            }, null, cancellationToken);
        }
        else if (request.PaymentType == PaymentType.FreeDelivery)
        {
            await _freeDelivery.SendAsync(new FreeDelivery
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId,
                PaymentType = request.PaymentType
            }, null, cancellationToken);
        }
        else
        {
            request.PaymentType = PaymentType.CardPayment;
            await _cardPayment.SendAsync(new CardPayment
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId,
                PaymentType = request.PaymentType
            }, null, cancellationToken);
        }
        return GenericResponse<CreateDeliveryResponse>.Success(new CreateDeliveryResponse { }, 200);
    }
}