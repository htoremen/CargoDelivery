namespace Route.Application.Routes.StartRoutes;

public class StartRouteCommand : IRequest<GenericResponse<StartRouteResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class StartRouteCommandCommandHandler : IRequestHandler<StartRouteCommand, GenericResponse<StartRouteResponse>>
{
    public async Task<GenericResponse<StartRouteResponse>> Handle(StartRouteCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<StartRouteResponse>.Success(new StartRouteResponse { }, 200);
    }
}
