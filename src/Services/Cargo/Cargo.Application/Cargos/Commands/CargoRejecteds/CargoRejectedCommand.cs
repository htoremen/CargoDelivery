namespace Cargo.Application.Cargos.CargoRejecteds;

public class CargoRejectedCommand : IRequest<GenericResponse<CargoRejectedResponse>>
{
    public Guid CorrelationId { get; set; }
}

public class CargoRejectedCommandHandler : IRequestHandler<CargoRejectedCommand, GenericResponse<CargoRejectedResponse>>
{
    public async Task<GenericResponse<CargoRejectedResponse>> Handle(CargoRejectedCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<CargoRejectedResponse>.Success(200);
    }
}

