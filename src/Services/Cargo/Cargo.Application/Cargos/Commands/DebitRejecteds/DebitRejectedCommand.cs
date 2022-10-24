namespace Cargo.Application.Cargos.DebitRejecteds;

public class DebitRejectedCommand : IRequest<GenericResponse<DebitRejectedResponse>>
{
    public Guid CorrelationId { get; set; }
}

public class DebitRejectedCommandHandler : IRequestHandler<DebitRejectedCommand, GenericResponse<DebitRejectedResponse>>
{
    public async Task<GenericResponse<DebitRejectedResponse>> Handle(DebitRejectedCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<DebitRejectedResponse>.Success(200);
    }
}

