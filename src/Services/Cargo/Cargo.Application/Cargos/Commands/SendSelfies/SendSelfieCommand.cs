using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.SendSelfie;

public class SendSelfieCommand : IRequest<GenericResponse<SendSelfieResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
    public string Selfie { get; set; }
}

public class SendSelfieCommandHandler : IRequestHandler<SendSelfieCommand, GenericResponse<SendSelfieResponse>>
{
    private IApplicationDbContext _context;

    public SendSelfieCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<GenericResponse<SendSelfieResponse>> Handle(SendSelfieCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString());
        if(debit != null)
        {
            debit.Selfie = request.Selfie; // Base64
            debit.CurrentState = request.CurrentState;
            debit = _context.Debits.Update(debit).Entity;
            await _context.SaveChangesAsync(cancellationToken);
            return GenericResponse<SendSelfieResponse>.Success(new SendSelfieResponse { CorrelationId = request.CorrelationId, CurrentState = debit.CurrentState }, 200);
        }
        return GenericResponse<SendSelfieResponse>.NotFoundException("", 404);
    }
}