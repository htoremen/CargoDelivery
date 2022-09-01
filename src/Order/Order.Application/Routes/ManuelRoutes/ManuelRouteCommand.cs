using MassTransit;

namespace Order.Application.Routes.ManuelRoutes;

public class ManuelRouteCommand : IRequest<GenericResponse<ManuelRouteResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class ManuelRouteCommandHandler : IRequestHandler<ManuelRouteCommand, GenericResponse<ManuelRouteResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public ManuelRouteCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<ManuelRouteResponse>> Handle(ManuelRouteCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<IManuelRoute>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        return GenericResponse<ManuelRouteResponse>.Success(new ManuelRouteResponse { CargoId = request.CargoId }, 200);
    }
}
