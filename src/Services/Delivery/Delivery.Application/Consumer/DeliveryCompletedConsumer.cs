using Delivery.Application.Deliveries.DeliveryCompleteds;

namespace Delivery.Application.Consumer;
public class DeliveryCompletedConsumer : IConsumer<IDeliveryCompleted>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<INewDelivery> _newDelivery;

    public DeliveryCompletedConsumer(IMediator mediator, IMessageSender<INewDelivery> newDelivery)
    {
        _mediator = mediator;
        _newDelivery = newDelivery;
    }
    public async Task Consume(ConsumeContext<IDeliveryCompleted> context)
    {
        var command = context.Message;

        var response = await _mediator.Send(new DeliveryCompletedCommand
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState,
        });
        
        if (response.Data.IsDeliveryCompleted == false)
        {
            await _newDelivery.SendAsync(new NewDelivery
            {
                CorrelationId = command.CorrelationId,
                CurrentState = command.CurrentState
            }, null);
        }
    }
}
