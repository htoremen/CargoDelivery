using MassTransit;

namespace Order.Application.Routes.RouteConfirmeds;

public class RouteConfirmedCommand : IRequest<GenericResponse<RouteConfirmedResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class RouteConfirmedCommandHandler : IRequestHandler<RouteConfirmedCommand, GenericResponse<RouteConfirmedResponse>>
{
    private readonly IMessageSender<IRouteConfirmed> _routeConfirmed;

    public RouteConfirmedCommandHandler(IMessageSender<IRouteConfirmed> routeConfirmed)
    {
        _routeConfirmed = routeConfirmed;
    }

    public async Task<GenericResponse<RouteConfirmedResponse>> Handle(RouteConfirmedCommand request, CancellationToken cancellationToken)
    {
        await _routeConfirmed.SendAsync(new RouteConfirmed
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<RouteConfirmedResponse>.Success(new RouteConfirmedResponse { CargoId = request.CargoId }, 200);
    }
}
