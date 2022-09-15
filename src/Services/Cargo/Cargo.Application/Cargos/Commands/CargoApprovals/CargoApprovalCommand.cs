using Core.Domain.MessageBrokers;
using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class CargoApprovalCommandHandler : IRequestHandler<CargoApprovalCommand, GenericResponse<CargoApprovalResponse>>
{
    private readonly IMessageSender<IStartRoute> _startRoute;
    private readonly IMessageSender<ICargoRejected> _cargoRejected;
    private readonly IApplicationDbContext _context;

    public CargoApprovalCommandHandler(IMessageSender<IStartRoute> startRoute, IMessageSender<ICargoRejected> cargoRejected, IApplicationDbContext context)
    {
        _startRoute = startRoute;
        _cargoRejected = cargoRejected;
        _context = context;
    }

    public async Task<GenericResponse<CargoApprovalResponse>> Handle(CargoApprovalCommand request, CancellationToken cancellationToken)
    {
        await _startRoute.SendAsync(new StartRoute
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId,
        }, null, cancellationToken);

        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString());
        if (debit != null)
        {
            debit.CurrentState = request.CurrentState;
            debit = _context.Debits.Update(debit).Entity;
            await _context.SaveChangesAsync(cancellationToken);
        }

        //await _cargoRejected.SendAsync(new CargoRejected
        //{
        //    CargoId = request.CargoId,
        //    CorrelationId = request.CorrelationId
        //}, null, cancellationToken);

        return GenericResponse<CargoApprovalResponse>.Success(new CargoApprovalResponse { }, 200);
    }
}
