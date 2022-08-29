namespace Order.Application.Orders.OrderApproveds;

public class OrderApprovedCommand : IRequest<GenericResponse<OrderApprovedResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
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
        var orderApproved = new CargoSendApproved
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        };
        await _eventBusService.SendCommandAsync(orderApproved, _queueConfiguration.Names[QueueName.CargoSaga], cancellationToken);
        var response = new OrderApprovedResponse { Id = request.CargoId };

        return GenericResponse<OrderApprovedResponse>.Success(response, 200);
    }
}