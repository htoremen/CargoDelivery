using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.Commands.UpdateStates;

public class UpdateStateCommand : IRequest<GenericResponse<UpdateStateResponse>>
{
    public string CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class UpdateStateCommandHandler : IRequestHandler<UpdateStateCommand, GenericResponse<UpdateStateResponse>>
{
    private IApplicationDbContext _context;

    public UpdateStateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<UpdateStateResponse>> Handle(UpdateStateCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString());
        if(debit != null)
        {
            debit.CurrentState = request.CurrentState;
            _context.Debits.Update(debit);
            await _context.SaveChangesAsync(cancellationToken);
            return GenericResponse<UpdateStateResponse>.Success(200);
        }
        return GenericResponse<UpdateStateResponse>.NotFoundException("", 404);
    }
}
