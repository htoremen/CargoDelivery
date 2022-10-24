using AutoMapper;
namespace Route.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }

    public string CurrentRouteAddress { get; set; }
    public List<ManuelRouteModel> Routes { get; set; }
}
public class ManuelRouteCommandHandler : IRequestHandler<ManuelRouteCommand, GenericResponse<ManuelRouteResponse>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public ManuelRouteCommandHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GenericResponse<ManuelRouteResponse>> Handle(ManuelRouteCommand request, CancellationToken cancellationToken)
    {
        var startRoute = await _context.Routes.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.IsCurrentRoute == true);
        if (startRoute == null)
        {
            startRoute = _context.Routes.Add(new Domain.Entities.Route
            {
                RouteId = Guid.NewGuid().ToString(),
                CorrelationId = request.CorrelationId.ToString(),
                RouteAddress = request.CurrentRouteAddress,
                IsCurrentRoute = true
            }).Entity;

            await _context.SaveChangesAsync(cancellationToken);
        }

        foreach (var cargoRoute in request.Routes)
        {
            var cargo = await _context.Cargos.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.CargoId == cargoRoute.CargoId);
            if(cargo == null)
            {
                cargo = _context.Cargos.Add(new Domain.Entities.Cargo
                {
                    CargoId = cargoRoute.CargoId,
                    CorrelationId = request.CorrelationId.ToString(),
                    CreatedOn = DateTime.Now,
                    RouteSequence = cargoRoute.OrderNo,
                    StartRouteId = startRoute.RouteId,
                    EndRoute = new Domain.Entities.Route
                    {
                        RouteId = Guid.NewGuid().ToString(),
                        CorrelationId = request.CorrelationId.ToString(),
                        IsCurrentRoute = false,
                        RouteAddress = cargoRoute.RouteAddress
                    }
                }).Entity;

                await _context.SaveChangesAsync(cancellationToken);
            }
            startRoute = cargo.EndRoute;
        }

        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { }, 200);
    }
}