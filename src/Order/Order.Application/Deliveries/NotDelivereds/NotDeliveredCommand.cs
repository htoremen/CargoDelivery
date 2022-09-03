using Deliveries;
using MassTransit;

namespace Delivery.Application.Deliveries.NotDelivereds;

public class NotDeliveredCommand : IRequest<GenericResponse<NotDeliveredResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class NotDeliveredCommandHandler : IRequestHandler<NotDeliveredCommand, GenericResponse<NotDeliveredResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public NotDeliveredCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<NotDeliveredResponse>> Handle(NotDeliveredCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<INotDelivered>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<NotDeliveredResponse>.Success(new NotDeliveredResponse { CargoId = request.CargoId }, 200);

    }
}
