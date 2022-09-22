using Delivery.Application.Deliveries.NotDelivereds;

namespace Delivery.Application.Consumer;

public class NotDeliveredConsumer : IConsumer<INotDelivered>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public NotDeliveredConsumer(IMediator mediator, IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _mediator = mediator;
        _deliveryCompleted = deliveryCompleted;
    }
    public async Task Consume(ConsumeContext<INotDelivered> context)
    {
        var command = context.Message;

        await _mediator.Send(new NotDeliveredCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
            CurrentState = command.CurrentState,
            DeliveryType = DeliveryType.NotDelivered
        });

        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState,
            CargoId = command.CargoId,
        }, null);
    }
}
