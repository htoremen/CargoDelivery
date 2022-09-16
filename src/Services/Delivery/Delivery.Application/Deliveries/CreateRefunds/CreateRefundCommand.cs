

namespace Delivery.Application.Deliveries.CreateRefunds;

public class CreateRefundCommand : IRequest<GenericResponse<CreateRefundResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class CreateRefundCommandHandler : IRequestHandler<CreateRefundCommand, GenericResponse<CreateRefundResponse>>
{
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public CreateRefundCommandHandler(IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _deliveryCompleted = deliveryCompleted;
    }

    public async Task<GenericResponse<CreateRefundResponse>> Handle(CreateRefundCommand request, CancellationToken cancellationToken)
    {
        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CurrentState = request.CurrentState,
            CorrelationId = request.CorrelationId

        }, null, cancellationToken);
        return GenericResponse<CreateRefundResponse>.Success(new CreateRefundResponse { }, 200);

    }
}

