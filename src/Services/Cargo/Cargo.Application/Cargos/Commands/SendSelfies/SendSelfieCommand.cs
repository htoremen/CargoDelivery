namespace Cargo.Application.Cargos.SendSelfie;

public class SendSelfieCommand : IRequest<GenericResponse<SendSelfieResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class SendSelfieCommandHandler : IRequestHandler<SendSelfieCommand, GenericResponse<SendSelfieResponse>>
{
    private IApplicationDbContext _context;
    private IMongoRepository<DebitBson> _debitRepository;

    public SendSelfieCommandHandler(IApplicationDbContext context, IMongoRepository<DebitBson> debitRepository)
    {
        _context = context;
        _debitRepository = debitRepository;
    }
    public async Task<GenericResponse<SendSelfieResponse>> Handle(SendSelfieCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<SendSelfieResponse>.Success(new SendSelfieResponse { }, 200);
    }
}