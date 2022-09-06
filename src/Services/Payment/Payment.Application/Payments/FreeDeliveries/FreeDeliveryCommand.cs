using Deliveries;
using MassTransit;

namespace Payment.Application.Payments.FreeDeliveries;

public class FreeDeliveryCommand : IRequest<GenericResponse<FreeDeliveryResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
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
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId

        }, null, cancellationToken);
        return GenericResponse<FreeDeliveryResponse>.Success(new FreeDeliveryResponse { }, 200);
    }
}