using MassTransit;

namespace Order.Application.Payments.PayAtDoors;

public class PayAtDoorCommand : IRequest<GenericResponse<PayAtDoorResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class PayAtDoorCommandHandler : IRequestHandler<PayAtDoorCommand, GenericResponse<PayAtDoorResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public PayAtDoorCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<PayAtDoorResponse>> Handle(PayAtDoorCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<IPayAtDoor>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<PayAtDoorResponse>.Success(new PayAtDoorResponse { CargoId = request.CargoId }, 200);

    }
}