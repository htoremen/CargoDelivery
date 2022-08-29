
namespace Order.Application.Orders.CreateOrders;

public class CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public Guid Id { get; set; }
    public Guid CargoId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IEventBusService<IEventBus> _eventBusService;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateOrderCommandHandler(IEventBusService<IEventBus> eventBusService, IQueueConfiguration queueConfiguration)
    {
        _eventBusService = eventBusService;
        _queueConfiguration = queueConfiguration;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var createOrderEvent = new CreateCargo
        {
            CustomerId = request.CustomerId,
            CargoId = request.CargoId,
            ProductId = request.ProductId
        };
        await _eventBusService.SendCommandAsync(createOrderEvent, _queueConfiguration.Names[QueueName.CargoSaga], cancellationToken);
        return new CreateOrderResponse { Id = request.Id, CargoId = request.CargoId };
    }
}