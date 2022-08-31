using MassTransit;

namespace Order.Application.Routes.RouteConfirmeds;

public class RouteConfirmedCommand : IRequest<GenericResponse<RouteConfirmedResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class RouteConfirmedCommandHandler : IRequestHandler<RouteConfirmedCommand, GenericResponse<RouteConfirmedResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public RouteConfirmedCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.RouteSaga]}")).Result;
    }

    public async Task<GenericResponse<RouteConfirmedResponse>> Handle(RouteConfirmedCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<IRouteConfirmed>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<RouteConfirmedResponse>.Success(new RouteConfirmedResponse { CargoId = request.CargoId }, 200);
    }
}
