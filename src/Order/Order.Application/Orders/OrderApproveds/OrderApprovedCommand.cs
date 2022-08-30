using MassTransit;

namespace Order.Application.Orders.OrderApproveds;

public class OrderApprovedCommand : IRequest<GenericResponse<OrderApprovedResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class OrderApprovedCommandHandler : IRequestHandler<OrderApprovedCommand, GenericResponse<OrderApprovedResponse>>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public OrderApprovedCommandHandler(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public async Task<GenericResponse<OrderApprovedResponse>> Handle(OrderApprovedCommand request, CancellationToken cancellationToken)
    {
        await _sendEndpoint.Send<ICargoSendApproved>(new
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, cancellationToken);
        var response = new OrderApprovedResponse { Id = request.CargoId };

        return GenericResponse<OrderApprovedResponse>.Success(response, 200);
    }
}