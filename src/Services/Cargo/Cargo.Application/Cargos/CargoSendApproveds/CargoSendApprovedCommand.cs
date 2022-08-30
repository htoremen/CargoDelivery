using Core.Domain.Enums;

namespace Cargo.Application.Cargos.CargoSendApproveds;

public class CargoSendApprovedCommand : IRequest<GenericResponse<CargoSendApprovedResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CargoSendApprovedCommandHandler : IRequestHandler<CargoSendApprovedCommand, GenericResponse<CargoSendApprovedResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CargoSendApprovedCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<CargoSendApprovedResponse>> Handle(CargoSendApprovedCommand request, CancellationToken cancellationToken)
    {
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            await _sendEndpoint.Send<ICargoApproved>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
                
            }, cancellationToken);
        }
        else
        {
            await _sendEndpoint.Send< ICargoRejected>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
        }

        return GenericResponse<CargoSendApprovedResponse>.Success(new CargoSendApprovedResponse { }, 200);
    }
}
