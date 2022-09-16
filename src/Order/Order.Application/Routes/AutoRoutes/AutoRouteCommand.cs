namespace Order.Application.Routes.AutoRoutes;

public class AutoRouteCommand : IRequest<GenericResponse<AutoRouteResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
public class AutoRouteCommandHandler : IRequestHandler<AutoRouteCommand, GenericResponse<AutoRouteResponse>>
{
    private readonly IMessageSender<IAutoRoute> _autoRoute;

    public AutoRouteCommandHandler(IMessageSender<IAutoRoute> autoRoute)
    {
        _autoRoute = autoRoute;
    }

    public async Task<GenericResponse<AutoRouteResponse>> Handle(AutoRouteCommand request, CancellationToken cancellationToken)
    {
        await _autoRoute.SendAsync(new AutoRoute
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<AutoRouteResponse>.Success(new AutoRouteResponse { CurrentState = request.CurrentState }, 200);
    }
}
