namespace Order.Application.Orders.CreateSelfies;

public class CreateSelfieCommand : IRequest<GenericResponse<CreateSelfeiResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CreateSelfeiCommandHandler : IRequestHandler<CreateSelfieCommand, GenericResponse<CreateSelfeiResponse>>
{
    private readonly IEventBusService<IEventBus> _eventBusService;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateSelfeiCommandHandler(IEventBusService<IEventBus> eventBusService, IQueueConfiguration queueConfiguration)
    {
        _eventBusService = eventBusService;
        _queueConfiguration = queueConfiguration;
    }

    public async Task<GenericResponse<CreateSelfeiResponse>> Handle(CreateSelfieCommand request, CancellationToken cancellationToken)
    {
        var createSelfei = new CreateSelfie
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        };
        await _eventBusService.SendCommandAsync(createSelfei, _queueConfiguration.Names[QueueName.CargoSaga], cancellationToken);
        var response = new CreateSelfeiResponse { Id = request.CargoId };

        return GenericResponse<CreateSelfeiResponse>.Success(response, 200);
    }
}

