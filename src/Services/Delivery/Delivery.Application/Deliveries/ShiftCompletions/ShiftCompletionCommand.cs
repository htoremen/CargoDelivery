
using MassTransit;

namespace Delivery.Application.Deliveries.ShiftCompletions;

public class ShiftCompletionCommand : IRequest<GenericResponse<ShiftCompletionResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}


public class ShiftCompletionCommandHandler : IRequestHandler<ShiftCompletionCommand, GenericResponse<ShiftCompletionResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public ShiftCompletionCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<ShiftCompletionResponse>> Handle(ShiftCompletionCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<IShiftCompletion>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<ShiftCompletionResponse>.Success(new ShiftCompletionResponse { CargoId = request.CargoId }, 200);
    }
}

