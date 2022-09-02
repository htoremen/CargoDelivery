using Deliveries;
using MassTransit;

namespace Order.Application.Deliveries.CreateRefunds;

public class CreateRefundCommand : IRequest<GenericResponse<CreateRefundResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CreateRefundCommandHandler : IRequestHandler<CreateRefundCommand, GenericResponse<CreateRefundResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateRefundCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<CreateRefundResponse>> Handle(CreateRefundCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<ICreateRefund>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<CreateRefundResponse>.Success(new CreateRefundResponse { CargoId = request.CargoId }, 200);

    }
}
