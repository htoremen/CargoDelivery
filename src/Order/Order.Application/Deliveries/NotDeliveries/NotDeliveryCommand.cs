using Deliveries;
using MassTransit;

namespace Order.Application.Deliveries.NotDeliveries;

public class NotDeliveryCommand : IRequest<GenericResponse<NotDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class NotDeliveryCommandHandler : IRequestHandler<NotDeliveryCommand, GenericResponse<NotDeliveryResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public NotDeliveryCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<NotDeliveryResponse>> Handle(NotDeliveryCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<INotDelivery>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<NotDeliveryResponse>.Success(new NotDeliveryResponse { CargoId = request.CargoId }, 200);

    }
}
