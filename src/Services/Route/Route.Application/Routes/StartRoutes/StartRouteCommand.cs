using Core.Domain.Enums;

namespace Route.Application.Routes.StartRoutes;

public class StartRouteCommand : IRequest<GenericResponse<StartRouteResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class StartRouteCommandCommandHandler : IRequestHandler<StartRouteCommand, GenericResponse<StartRouteResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public StartRouteCommandCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<StartRouteResponse>> Handle(StartRouteCommand request, CancellationToken cancellationToken)
    {
        

        return GenericResponse<StartRouteResponse>.Success(new StartRouteResponse { }, 200);
    }
}
