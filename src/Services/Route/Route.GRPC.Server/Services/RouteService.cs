using Grpc.Core;
using MediatR;

namespace Route.GRPC.Server.Services;

public class RouteService : RouteGrpc.RouteGrpcBase
{
    private readonly IMediator _mediator;

    public RouteService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetRouteResponse> GetRoute(GetRouteRequest request, ServerCallContext context)
    {

    }
}
