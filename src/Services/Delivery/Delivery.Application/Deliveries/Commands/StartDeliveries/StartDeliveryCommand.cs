

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

    public StartDeliveryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<StartDeliveryResponse>> Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
    {
        foreach (var cargo in request.Cargos)
        {
            var route = request.Routes.FirstOrDefault(x => x.CargoId == cargo.CargoId);
            _context.Cargos.Add(new Domain.Entities.Cargo
            {
                Address = cargo.Address,
                CargoId = cargo.CargoId,
                DebitId = cargo.DebitId,
                Route = route.Route,
                CargoItems = cargo.CargoItems.Select(y => new Domain.Entities.CargoItem
                {
                    Barcode = y.Barcode,
                    CargoItemId = y.CargoItemId,
                    CargoId = cargo.CargoId,
                    Description = y.Description,
                    Desi = y.Desi,
                    Kg = y.Kg,
                    WaybillNumber = y.WaybillNumber
                }).ToList()
            });
        }

        await _context.SaveChangesAsync(cancellationToken);
        return GenericResponse<StartDeliveryResponse>.Success(new StartDeliveryResponse { }, 200);
    }
}

