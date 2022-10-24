

namespace Route.Application.Routes.StartRoutes;

public class StartRouteCommand : IRequest<GenericResponse<StartRouteResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class StartRouteCommandCommandHandler : IRequestHandler<StartRouteCommand, GenericResponse<StartRouteResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public StartRouteCommandCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<GenericResponse<StartRouteResponse>> Handle(StartRouteCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = StaticKeyValues.CreateDebit + request.CorrelationId.ToString();
        var data = await _cache.GetValueAsync(cacheKey);
        var response = JsonSerializer.Deserialize<CreateDebitModel>(data);

        foreach (var item in response.Cargos)
        {
            var cargoRoute = await _context.Cargos.FirstOrDefaultAsync(x => x.CargoId == item.CargoId.ToString() && x.CorrelationId == request.CorrelationId.ToString());
            if (cargoRoute == null)
            {
                _context.Cargos.Add(new Domain.Entities.Cargo
                {
                    CargoId = item.CargoId.ToString(),
                    CorrelationId = request.CorrelationId.ToString(),
                    CreatedOn = DateTime.Now
                });
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        return GenericResponse<StartRouteResponse>.Success(new StartRouteResponse { CorrelationId = request.CorrelationId }, 200);
    }

    public async Task<GenericResponse<StartRouteResponse>> Handle_(StartRouteCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = StaticKeyValues.CreateDebit + request.CorrelationId.ToString();
        var data = await _cache.GetValueAsync(cacheKey);
        var response = JsonSerializer.Deserialize<CreateDebitModel>(data);

        foreach (var item in response.Cargos)
        {
            var cargoRoute = await _context.CargoRoutes.FirstOrDefaultAsync(x => x.CargoId == item.CargoId.ToString() && x.CorrelationId == request.CorrelationId.ToString());
            if (cargoRoute == null)
            {
                _context.CargoRoutes.Add(new CargoRoute
                {
                    CargoRouteId = Guid.NewGuid().ToString(),
                    Route = "",
                    CargoId = item.CargoId.ToString(),
                    CorrelationId = request.CorrelationId.ToString(),
                    Address = item.Address,
                    CreatedOn = DateTime.Now,
                    IsRoute = true,
                });
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        return GenericResponse<StartRouteResponse>.Success(new StartRouteResponse { CorrelationId = request.CorrelationId }, 200);
    }
}