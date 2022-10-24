using Core.Domain.SerializerModels;

namespace Order.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }

    public List<ManuelRouteModel> Routes { get; set; }
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
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId,
            Routes = request.Routes
        }, null, cancellationToken);
        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { CurrentState = request.CurrentState }, 200);
    }
}
