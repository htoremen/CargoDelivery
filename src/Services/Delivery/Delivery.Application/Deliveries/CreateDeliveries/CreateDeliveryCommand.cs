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
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateDeliveryCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<CreateDeliveryResponse>> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        if (request.PaymentType == PaymentType.CardPayment)
        {
            await _sendEndpoint.Send<ICardPayment>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
        }
        else if (request.PaymentType == PaymentType.PayAtDoor)
        {
            await _sendEndpoint.Send<IPayAtDoor>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
        }
        else if (request.PaymentType == PaymentType.FreeDelivery)
        {
            await _sendEndpoint.Send<IFreeDelivery>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
        }
        return GenericResponse<CreateDeliveryResponse>.Success(new CreateDeliveryResponse { }, 200);
    }
}