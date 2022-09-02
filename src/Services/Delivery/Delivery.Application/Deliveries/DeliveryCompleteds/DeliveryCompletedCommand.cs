using Core.Domain.Enums;

namespace Delivery.Application.Deliveries.DeliveryCompleteds;

public class DeliveryCompletedCommand : IRequest<GenericResponse<DeliveryCompletedResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class DeliveryCompletedCommandHandler : IRequestHandler<DeliveryCompletedCommand, GenericResponse<DeliveryCompletedResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public DeliveryCompletedCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<DeliveryCompletedResponse>> Handle(DeliveryCompletedCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<IStartDelivery>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId

        }, cancellationToken);
        return GenericResponse<DeliveryCompletedResponse>.Success(new DeliveryCompletedResponse { }, 200);
    }
}
