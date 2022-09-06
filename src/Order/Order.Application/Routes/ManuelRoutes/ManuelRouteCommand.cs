using MassTransit;

namespace Order.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class ManuelRouteCommandHandler : IRequestHandler<ManuelRouteCommand, GenericResponse<ManuelRouteResponse>>
{
    private readonly IMessageSender<IManuelRoute> _manuelRoute;

    public ManuelRouteCommandHandler(IMessageSender<IManuelRoute> manuelRoute)
    {
        _manuelRoute = manuelRoute;
    }

    public async Task<GenericResponse<ManuelRouteResponse>> Handle(ManuelRouteCommand request, CancellationToken cancellationToken)
    {
        await _manuelRoute.SendAsync(new ManuelRoute
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { CargoId = request.CargoId }, 200);
    }
}
