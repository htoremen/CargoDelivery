using Core.Domain.Enums;

namespace Cargo.Application.Cargos.CargoApproveds;

public class CargoApprovedCommand : IRequest<GenericResponse<CargoApprovedResponse>>
{
    public Guid CorrelationId { get; set; }
}

public class OrderApprovedCommandHandler : IRequestHandler<CargoApprovedCommand, GenericResponse<CargoApprovedResponse>>
{
    public async Task<GenericResponse<CargoApprovedResponse>> Handle(CargoApprovedCommand request, CancellationToken cancellationToken)
    {

        return GenericResponse<CargoApprovedResponse>.Success(new CargoApprovedResponse { }, 200);
    }
}
