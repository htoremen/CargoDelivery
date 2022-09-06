using Core.Domain.MessageBrokers;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CargoApprovalCommandHandler : IRequestHandler<CargoApprovalCommand, GenericResponse<CargoApprovalResponse>>
{
    private readonly IMessageSender<ICargoApproval> _cargoApproval;

    public CargoApprovalCommandHandler(IMessageSender<ICargoApproval> cargoApproval)
    {
        _cargoApproval = cargoApproval;
    }

    public async Task<GenericResponse<CargoApprovalResponse>> Handle(CargoApprovalCommand request, CancellationToken cancellationToken)
    {
        await _cargoApproval.SendAsync(new CargoApproval
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        var response = new CargoApprovalResponse { CargoId = request.CargoId };

        return GenericResponse<CargoApprovalResponse>.Success(response, 200);
    }
}