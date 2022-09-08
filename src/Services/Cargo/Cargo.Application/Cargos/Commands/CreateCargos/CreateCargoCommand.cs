using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.CreateCargos;

public class CreateCargoCommand : IRequest<GenericResponse<CreateCargoResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CargoItemRequest> CargoItems { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, GenericResponse<CreateCargoResponse>>
{
    private IApplicationDbContext _context;

    public CreateCargoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<CreateCargoResponse>> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.DistributionDate != DateTime.Now && x.CorrelationId == request.CorrelationId.ToString() && x.IsCompleted == false);
        if(debit == null)
        {
            debit = _context.Debits.Add(new Debit
            {
                DebitId = request.DebitId.ToString(),
                CorrelationId = request.CorrelationId.ToString(),
                CourierId = request.CourierId.ToString(),
                DistributionDate = DateTime.Now.Date,
                StartingDate = DateTime.Now,
                IsApproval = false,
                IsCompleted = false
            }).Entity;
            await _context.SaveChangesAsync(cancellationToken);
        }
        return GenericResponse<CreateCargoResponse>.Success(new CreateCargoResponse { DebitId = Guid.Parse(debit.DebitId), CorrelationId = Guid.Parse(debit.CorrelationId)  }, 200);
    }
}
