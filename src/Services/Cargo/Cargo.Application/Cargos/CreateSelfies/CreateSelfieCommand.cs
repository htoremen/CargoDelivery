namespace Cargo.Application.Cargos.CreateSelfies;

public class CreateSelfieCommand : IRequest<GenericResponse<CreateSelfieResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CreateSelfieCommandhandler : IRequestHandler<CreateSelfieCommand, GenericResponse<CreateSelfieResponse>>
{
    public async Task<GenericResponse<CreateSelfieResponse>> Handle(CreateSelfieCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<CreateSelfieResponse>.Success(new CreateSelfieResponse { }, 200);
    }
}