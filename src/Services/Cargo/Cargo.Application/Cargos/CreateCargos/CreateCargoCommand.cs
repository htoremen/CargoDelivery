

namespace Cargo.Application.Cargos.CreateCargos;

public class CreateCargoCommand : IRequest<GenericResponse<CreateCargoResponse>>
{
    public Guid Id { get; set; }
    public Guid CargoId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, GenericResponse<CreateCargoResponse>>
{
    public async Task<GenericResponse<CreateCargoResponse>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<CreateCargoResponse>.Success(new CreateCargoResponse {  }, 200);
    }
}
