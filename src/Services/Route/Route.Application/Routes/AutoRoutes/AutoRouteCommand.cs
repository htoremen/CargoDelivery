namespace Route.Application.Routes.AutoRoutes;

public class AutoRouteCommand : IRequest<GenericResponse<AutoRouteResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class AutoRouteCommandHandler : IRequestHandler<AutoRouteCommand, GenericResponse<AutoRouteResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public AutoRouteCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<AutoRouteResponse>> Handle(AutoRouteCommand request, CancellationToken cancellationToken)
    {
        var rnd = new Random();
        if (rnd.Next(1, 1000) % 2 == 0)
        {
            //await _sendEndpoint.Send<ICargoApproval>(new
            //{
            //    CargoId = request.CargoId,
            //    CorrelationId = request.CorrelationId

            //}, cancellationToken);
            await _sendEndpoint.Send<IStartDelivery>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
        }
        else
        {
            await _sendEndpoint.Send<IStartDelivery>(new
            {
                CargoId = request.CargoId,
                CorrelationId = request.CorrelationId
            }, cancellationToken);
        }

        return GenericResponse<AutoRouteResponse>.Success(new AutoRouteResponse { }, 200);
    }
}

