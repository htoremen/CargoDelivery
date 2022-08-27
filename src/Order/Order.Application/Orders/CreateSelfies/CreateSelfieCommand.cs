﻿namespace Order.Application.Orders.CreateSelfies;

public class CreateSelfieCommand : IRequest<Response<CreateSelfeiResponse>>
{
    public Guid Id { get; set; }
}

public class CreateSelfeiCommandHandler : IRequestHandler<CreateSelfieCommand, Response<CreateSelfeiResponse>>
{
    private readonly IEventBusService<IEventBus> _eventBusService;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateSelfeiCommandHandler(IEventBusService<IEventBus> eventBusService, IQueueConfiguration queueConfiguration)
    {
        _eventBusService = eventBusService;
        _queueConfiguration = queueConfiguration;
    }

    public async Task<Response<CreateSelfeiResponse>> Handle(CreateSelfieCommand request, CancellationToken cancellationToken)
    {
        var createSelfei = new CreateSelfie
        {
            Id = request.Id,
        };
        await _eventBusService.SendCommandAsync(createSelfei, _queueConfiguration.Names[QueueState.CreateSelfie], cancellationToken);
        var response = new CreateSelfeiResponse { Id = request.Id };

        return Response<CreateSelfeiResponse>.Success(response, 200);
    }
}

