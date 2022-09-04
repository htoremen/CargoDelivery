using MassTransit;

namespace Order.Application.Payments.FreeDeliveries;

public class FreeDeliveryCommand : IRequest<GenericResponse<FreeDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class FreeDeliveryCommandHandler : IRequestHandler<FreeDeliveryCommand, GenericResponse<FreeDeliveryResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public FreeDeliveryCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<FreeDeliveryResponse>> Handle(FreeDeliveryCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<IFreeDelivery>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<FreeDeliveryResponse>.Success(new FreeDeliveryResponse { CargoId = request.CargoId }, 200);

    }
}
