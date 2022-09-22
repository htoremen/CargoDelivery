

namespace Delivery.Application.Deliveries.StartDeliveries;

public class StartDeliveryCommand : IRequest<GenericResponse<StartDeliveryResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public List<GetCargos> Cargos { get; internal set; }
    public List<GetRouteDataResponse> Routes { get; set; }
}

public class StartDeliveryCommandHandler : IRequestHandler<StartDeliveryCommand, GenericResponse<StartDeliveryResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDebitService _debitService;

    public StartDeliveryCommandHandler(IApplicationDbContext context, IDebitService debitService)
    {
        _context = context;
        _debitService = debitService;
    }

    public async Task<GenericResponse<StartDeliveryResponse>> Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Cargos)
        {
            var route = request.Routes.FirstOrDefault(x => x.CargoId == item.CargoId);
            if (route == null) continue;

            var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.CargoId == item.CargoId);
            if (cargo == null)
            {
                _context.Cargos.Add(new Domain.Entities.Cargo
                {
                    Address = item.Address,
                    CargoId = item.CargoId,
                    DebitId = item.DebitId,
                    CorrelationId = item.CorrelationId,
                    Route = route.Route,
                });
                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var citem in item.CargoItems)
            {
                var cargoItem = await _context.CargoItems.FirstOrDefaultAsync(x => x.CargoItemId == citem.CargoItemId && x.CargoId == citem.CargoId && x.Cargo.CorrelationId == request.CorrelationId.ToString());
                if(cargoItem == null)
                {
                    cargoItem = _context.CargoItems.Add(new Domain.Entities.CargoItem
                    {
                        Barcode = citem.Barcode,
                        CargoItemId = citem.CargoItemId,
                        CargoId = item.CargoId,
                        Description = citem.Description,
                        Desi = citem.Desi,
                        Kg = citem.Kg,
                        WaybillNumber = citem.WaybillNumber
                    }).Entity;
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
        await _debitService.UpdateStateAsync(request.CorrelationId.ToString(), request.CurrentState);

        return GenericResponse<StartDeliveryResponse>.Success(new StartDeliveryResponse { }, 200);
    }
}

