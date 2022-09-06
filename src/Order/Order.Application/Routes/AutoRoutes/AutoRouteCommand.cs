using MassTransit;

namespace Order.Application.Routes.AutoRoutes;

public class AutoRouteCommand : IRequest<GenericResponse<AutoRouteResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
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
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<AutoRouteResponse>.Success(new AutoRouteResponse { CargoId = request.CargoId }, 200);
    }
}
