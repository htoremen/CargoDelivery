
using MassTransit;

namespace Cargo.Application.Orders.CreateOrders;

public class CreateCargoCommand : IRequest<CreateCargoResponse>
{
    public Guid Id { get; set; }
    public Guid CargoId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}

public class CreateCargoCommandHandler : IRequestHandler<CreateCargoCommand, CreateCargoResponse>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public CreateCargoCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<CreateCargoResponse> Handle(CreateCargoCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<ICreateCargo>(new
        {
            CustomerId = request.CustomerId,
            CargoId = request.CargoId,
            ProductId = request.ProductId
        }, cancellationToken);
        return new CreateCargoResponse { Id = request.Id, CargoId = request.CargoId };
    }
}