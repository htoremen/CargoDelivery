namespace Cargo.Application.Cargos.SendSelfie;

public class SendSelfieCommand : IRequest<GenericResponse<SendSelfieResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class SendSelfieCommandHandler : IRequestHandler<SendSelfieCommand, GenericResponse<SendSelfieResponse>>
{
    public async Task<GenericResponse<SendSelfieResponse>> Handle(SendSelfieCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<SendSelfieResponse>.Success(new SendSelfieResponse { }, 200);
    }
}

