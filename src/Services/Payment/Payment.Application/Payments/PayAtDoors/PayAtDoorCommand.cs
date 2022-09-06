﻿using Deliveries;

namespace Payment.Application.Payments.PayAtDoors;

public class PayAtDoorCommand : IRequest<GenericResponse<PayAtDoorResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
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
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId

        }, null, cancellationToken);
        return GenericResponse<PayAtDoorResponse>.Success(new PayAtDoorResponse { }, 200);
    }
}
