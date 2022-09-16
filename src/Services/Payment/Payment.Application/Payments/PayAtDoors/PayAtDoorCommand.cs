using Deliveries;

namespace Payment.Application.Payments.PayAtDoors;

public class PayAtDoorCommand : IRequest<GenericResponse<PayAtDoorResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class PayAtDoorCommandHandler : IRequestHandler<PayAtDoorCommand, GenericResponse<PayAtDoorResponse>>
{
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public PayAtDoorCommandHandler(IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _deliveryCompleted = deliveryCompleted;
    }
    public async Task<GenericResponse<PayAtDoorResponse>> Handle(PayAtDoorCommand request, CancellationToken cancellationToken)
    {
        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId,
            CargoId = request.CargoId,
        }, null, cancellationToken);
        return GenericResponse<PayAtDoorResponse>.Success(new PayAtDoorResponse { }, 200);
    }
}
