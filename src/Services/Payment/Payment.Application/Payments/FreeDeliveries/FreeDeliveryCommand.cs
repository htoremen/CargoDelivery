using Deliveries;
using MassTransit;

namespace Payment.Application.Payments.FreeDeliveries;

public class FreeDeliveryCommand : IRequest<GenericResponse<FreeDeliveryResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
public class FreeDeliveryCommandHandler : IRequestHandler<FreeDeliveryCommand, GenericResponse<FreeDeliveryResponse>>
{
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public FreeDeliveryCommandHandler(IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _deliveryCompleted = deliveryCompleted;
    }
    public async Task<GenericResponse<FreeDeliveryResponse>> Handle(FreeDeliveryCommand request, CancellationToken cancellationToken)
    {
        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId,
            CargoId = request.CargoId            
        }, null, cancellationToken);
        return GenericResponse<FreeDeliveryResponse>.Success(new FreeDeliveryResponse { }, 200);
    }
}