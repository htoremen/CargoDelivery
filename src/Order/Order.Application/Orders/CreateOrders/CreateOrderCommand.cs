
using MassTransit;

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
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateOrderCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<ICreateCargo>(new
        {
            CustomerId = request.CustomerId,
            CargoId = request.CargoId,
            ProductId = request.ProductId
        }, cancellationToken);
        return new CreateOrderResponse { Id = request.Id, CargoId = request.CargoId };
    }
}