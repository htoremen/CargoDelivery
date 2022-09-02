using Deliveries;
using MassTransit;

namespace Order.Application.Deliveries.CreateDeliveries;

public class CreateDeliveryCommand : IRequest<GenericResponse<CreateDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
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
        await _sendEndpoint.Send<ICreateDelivery>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<CreateDeliveryResponse>.Success(new CreateDeliveryResponse { CargoId = request.CargoId }, 200);
    }
}
