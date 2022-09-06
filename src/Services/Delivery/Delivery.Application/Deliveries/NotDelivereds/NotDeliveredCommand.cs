namespace Delivery.Application.Deliveries.NotDelivereds;

public class NotDeliveredCommand : IRequest<GenericResponse<NotDeliveredResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class NotDeliveredCommandHandler : IRequestHandler<NotDeliveredCommand, GenericResponse<NotDeliveredResponse>>
{
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public NotDeliveredCommandHandler(IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _deliveryCompleted = deliveryCompleted;
    }

    public async Task<GenericResponse<NotDeliveredResponse>> Handle(NotDeliveredCommand request, CancellationToken cancellationToken)
    {
        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId

        }, null, cancellationToken);
        return GenericResponse<NotDeliveredResponse>.Success(new NotDeliveredResponse { }, 200);
    }
}
