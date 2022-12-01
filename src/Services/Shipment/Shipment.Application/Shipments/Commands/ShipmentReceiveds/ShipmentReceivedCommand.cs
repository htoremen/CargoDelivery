
using Core.Application.Common.Interfaces;
using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.SerializerModels;
using Microsoft.EntityFrameworkCore;
using Shipment.Application.Common.Interfaces;
using System.Text.Json;

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
    private readonly ICacheService _cache;


    public ShipmentReceivedCommandHandler(IApplicationDbContext context, IMessageSender<IStartRoute> startRoute, IDebitService debitService, ICacheService cache)
    {
        _context = context;
        _startRoute = startRoute;
        _debitService = debitService;
        _cache = cache;
    }

    public async Task<Unit> Handle(ShipmentReceivedCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = StaticKeyValues.CreateDebit + request.CorrelationId.ToString();
        var data = await _cache.GetValueAsync(cacheKey);
        var response = JsonSerializer.Deserialize<CreateDebitModel>(data);
        foreach (var item in response.Cargos)
        {
            var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.DebitId == request.CorrelationId.ToString() && x.CargoId == item.CargoId.ToString());
            if (cargo == null)
            {
                _context.Cargos.Add(new Domain.Entities.Cargo
                {
                    CargoId = item.CargoId.ToString(),
                    DebitId = request.CorrelationId.ToString(),
                    CreateDate = DateTime.UtcNow,
                    ShipmentTypeId = (int)ShipmentType.ShipmentReceived
                });
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        await _debitService.UpdateStateAsync(request.CorrelationId.ToString(), request.CurrentState);

        await _startRoute.SendAsync(new StartRoute
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return Unit.Value;
    }
}
