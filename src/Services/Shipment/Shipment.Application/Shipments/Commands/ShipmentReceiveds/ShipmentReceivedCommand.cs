using Cargos;
using Core.Domain.Enums;
using Core.Domain.MessageBrokers;
using Microsoft.EntityFrameworkCore;
using Shipment.Application.Common.Interfaces;

namespace Shipment.Application.Shipments.Commands.ShipmentReceiveds;

public class ShipmentReceivedCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class ShipmentReceivedCommandHandler : IRequestHandler<ShipmentReceivedCommand>
{
    private readonly IApplicationDbContext _context; 
    private readonly IMessageSender<IStartRoute> _startRoute; 
    private readonly IDebitService _debitService;


    public ShipmentReceivedCommandHandler(IApplicationDbContext context, IMessageSender<IStartRoute> startRoute, IDebitService debitService)
    {
        _context = context;
        _startRoute = startRoute;
        _debitService = debitService;
    }

    public async Task<Unit> Handle(ShipmentReceivedCommand request, CancellationToken cancellationToken)
    {
        var cargos = new List<Domain.Entities.Cargo>();
        foreach (var item in cargos)
        {
            var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.DebitId == request.CorrelationId.ToString() && x.CargoId == item.CargoId);
            if (cargo == null)
            {
                _context.Cargos.Add(new Domain.Entities.Cargo
                {
                    CargoId = item.CargoId,
                    DebitId = item.DebitId,
                    CreateDate = DateTime.UtcNow,
                    ShipmentTypeId = (int)ShipmentType.ShipmentReceived
                });
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        await _startRoute.SendAsync(new StartRoute
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);

        await _debitService.UpdateStateAsync(request.CorrelationId.ToString(), request.CurrentState);

        return Unit.Value;
    }
}
