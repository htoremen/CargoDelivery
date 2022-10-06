namespace Cargo.Application.Cargos.Commands.UpdateCurrentStates;

public class UpdateCurrentStateCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
public class UpdateCurrentStateCommandHandler : IRequestHandler<UpdateCurrentStateCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCurrentStateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCurrentStateCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString());
        if (debit != null)
        {
            debit.CurrentState = request.CurrentState;
            debit = _context.Debits.Update(debit).Entity;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}