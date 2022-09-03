using MassTransit;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CargoApprovalCommandHandler : IRequestHandler<CargoApprovalCommand, GenericResponse<CargoApprovalResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CargoApprovalCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<CargoApprovalResponse>> Handle(CargoApprovalCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<ICargoApproval>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        var response = new CargoApprovalResponse { CargoId = request.CargoId };

        return GenericResponse<CargoApprovalResponse>.Success(response, 200);
    }
}