using AutoMapper;
namespace Route.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }

    public string CurrentRouteAddress { get; set; }
    public List<ManuelRouteRequest> CargoRoutes { get; set; }
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
        var currentRoute = await _context.Routes.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.IsCurrentRoute);
        if (currentRoute == null)
        {
            currentRoute = _context.Routes.Add(new Domain.Entities.Route
            {
                RouteId = Guid.NewGuid().ToString(),
                CorrelationId = request.CorrelationId.ToString(),
                RouteAddress = request.CurrentRouteAddress,
                IsCurrentRoute = true
            }).Entity;

            await _context.SaveChangesAsync(cancellationToken);
        }

        var cargoRoutes = request.CargoRoutes.OrderBy(x => x.OrderNo);
        foreach (var cargoRoute in cargoRoutes)
        {
            var route = await _context.Routes.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString());
            if (route == null)
            {
                route = _context.Routes.Add(new Domain.Entities.Route
                {
                    RouteId = Guid.NewGuid().ToString(),
                    CorrelationId = request.CorrelationId.ToString(),
                    RouteAddress = cargoRoute.RouteAddress,
                    IsCurrentRoute = false
                }).Entity;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { }, 200);
    }
}


