using Microsoft.EntityFrameworkCore;
using Route.Application.Common.Interfaces;

namespace Route.Application.Routes.Queries.GetRoutes;

public class GetRouteQuery : IRequest<GenericResponse<List<GetRouteResponse>>>
{
    public string CorrelationId { get; set; }
}

public class GetRouteQueryHandler : IRequestHandler<GetRouteQuery, GenericResponse<List<GetRouteResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetRouteQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<List<GetRouteResponse>>> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.CargoRoutes
            .Where(x => x.CorrelationId == request.CorrelationId)
            .Select(x => new GetRouteResponse
            {
                Address = x.Address,
                CargoId = x.CargoId,
                Route = x.Route,
            }).ToListAsync();
        return GenericResponse<List<GetRouteResponse>>.Success(result, 200);
    }
}