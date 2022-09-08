using Core.Domain.MessageBrokers;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CargoApprovalCommandHandler : IRequestHandler<CargoApprovalCommand, GenericResponse<CargoApprovalResponse>>
{
    private readonly IMessageSender<IStartRoute> _startRoute;
    private readonly IMessageSender<ICargoRejected> _cargoRejected;

    public CargoApprovalCommandHandler(IMessageSender<IStartRoute> startRoute, IMessageSender<ICargoRejected> cargoRejected)
    {
        _startRoute = startRoute;
        _cargoRejected = cargoRejected;
    }

    public async Task<GenericResponse<CargoApprovalResponse>> Handle(CargoApprovalCommand request, CancellationToken cancellationToken)
    {
        await _startRoute.SendAsync(new StartRoute
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);

        //await _cargoRejected.SendAsync(new CargoRejected
        //{
        //    CargoId = request.CargoId,
        //    CorrelationId = request.CorrelationId
        //}, null, cancellationToken);

        return GenericResponse<CargoApprovalResponse>.Success(new CargoApprovalResponse { }, 200);
    }
}
