namespace Order.Application.Orders.OrderApproveds;

public class OrderApprovedCommand : IRequest<GenericResponse<OrderApprovedResponse>>
{
    public Guid Id { get; set; }
}

public class OrderApprovedCommandHandler : IRequestHandler<OrderApprovedCommand, GenericResponse<OrderApprovedResponse>>
{
    private readonly IEventBusService<IEventBus> _eventBusService;
    private readonly IQueueConfiguration _queueConfiguration;

    public OrderApprovedCommandHandler(IEventBusService<IEventBus> eventBusService, IQueueConfiguration queueConfiguration)
    {
        _eventBusService = eventBusService;
        _queueConfiguration = queueConfiguration;
    }

    public async Task<GenericResponse<OrderApprovedResponse>> Handle(OrderApprovedCommand request, CancellationToken cancellationToken)
    {
        var orderApproved = new OrderApproved
        {
            Id = request.Id,
        };
        await _eventBusService.SendCommandAsync(orderApproved, _queueConfiguration.Names[QueueState.CargoApproved], cancellationToken);
        var response = new OrderApprovedResponse { Id = request.Id };

        return GenericResponse<OrderApprovedResponse>.Success(response, 200);
    }
}