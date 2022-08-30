using MassTransit;

namespace Order.Application.Orders.CreateSelfies;

public class CreateSelfieCommand : IRequest<GenericResponse<CreateSelfeiResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CreateSelfeiCommandHandler : IRequestHandler<CreateSelfieCommand, GenericResponse<CreateSelfeiResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateSelfeiCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<CreateSelfeiResponse>> Handle(CreateSelfieCommand request, CancellationToken cancellationToken)
    {
        var createSelfei = new CreateSelfie
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        };
        await _sendEndpoint.Send<ICreateSelfie>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        var response = new CreateSelfeiResponse { Id = request.CargoId };

        return GenericResponse<CreateSelfeiResponse>.Success(response, 200);
    }
}

