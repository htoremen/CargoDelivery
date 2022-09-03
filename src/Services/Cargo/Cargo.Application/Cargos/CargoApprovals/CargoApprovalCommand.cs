using Core.Domain.Enums;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
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
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            await _sendEndpoint.Send<IStartRoute>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId

            }, cancellationToken);
        }
        else
        {
            await _sendEndpoint.Send<IStartRoute>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId

            }, cancellationToken);

            //await _sendEndpoint.Send<ICargoRejected>(new
            //{
            //    CargoId = request.CargoId,
            //    CorrelationId = request.CorrelationId
            //}, cancellationToken);
        }

        return GenericResponse<CargoApprovalResponse>.Success(new CargoApprovalResponse { }, 200);
    }
}
