
namespace Delivery.Application.Cargos.Queries.GetRoutes.GetRouteQuery;

public class GetRouteQuery : IRequest<GenericResponse<GetRouteResponse>>
{
    public string CorrelationId { get; set; }
}

public class GetRouteQueryHandler : IRequestHandler<GetRouteQuery, GenericResponse<GetRouteResponse>>
{
    private readonly IRouteService _routeService;

    public GetRouteQueryHandler(IRouteService routeService)
    {
        _routeService = routeService;
    }

    public async Task<GenericResponse<GetRouteResponse>> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        var response = await _routeService.GetRouteAsync(request.CorrelationId);
        return GenericResponse<GetRouteResponse>.Success(response, 200);
    }
}