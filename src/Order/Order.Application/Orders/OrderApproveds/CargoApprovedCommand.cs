using MassTransit;

namespace Order.Application.Orders.OrderApproveds;

public class CargoApprovedCommand : IRequest<GenericResponse<CargoApprovedResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CargoApprovedCommandHandler : IRequestHandler<CargoApprovedCommand, GenericResponse<CargoApprovedResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CargoApprovedCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<CargoApprovedResponse>> Handle(CargoApprovedCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<ICargoSendApproved>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        var response = new CargoApprovedResponse { Id = request.CargoId };

        return GenericResponse<CargoApprovedResponse>.Success(response, 200);
    }
}