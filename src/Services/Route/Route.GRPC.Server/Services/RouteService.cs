using AutoMapper;
using Grpc.Core;
using MediatR;
using Route.Application.Routes.Queries.GetRoutes;

namespace Route.GRPC.Server.Services;

public class RouteService : RouteGrpc.RouteGrpcBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RouteService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<GetRouteResponse> GetRoute(GetRouteRequest request, ServerCallContext context)
    {
        var routes = await _mediator.Send(new GetRouteQuery { CorrelationId = request.CorrelationId });
        var response = new GetRouteResponse();
        foreach (var item in routes.Data)
        {
            response.Routes.Add(new GetRouteDataResponse
            {
                CargoId = item.CargoId,
                Address = item.Address,
                Route = item.Route
            });
        }
        return response;
    }
}