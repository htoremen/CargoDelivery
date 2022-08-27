namespace Order.Application.Orders.CreateSelfies;

public class CreateSelfieCommand : IRequest<GenericResponse<CreateSelfeiResponse>>
{
    public Guid Id { get; set; }
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
            Id = request.Id,
        };
        await _eventBusService.SendCommandAsync(createSelfei, _queueConfiguration.Names[QueueName.CreateSelfie], cancellationToken);
        var response = new CreateSelfeiResponse { Id = request.Id };

        return GenericResponse<CreateSelfeiResponse>.Success(response, 200);
    }
}

