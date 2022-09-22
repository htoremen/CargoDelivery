using Delivery.Application.Deliveries.DeliveryCompleteds;

namespace Delivery.Application.Consumer;
public class DeliveryCompletedConsumer : IConsumer<IDeliveryCompleted>
{
    private readonly IMediator _mediator;

    public DeliveryCompletedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<IDeliveryCompleted> context)
    {
        var command = context.Message;

        var response = await _mediator.Send(new DeliveryCompletedCommand
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState,
        });

        if(response.Data.IsDeliveryCompleted == false)
        {
            await context.Publish<INewDelivery>(new NewDelivery
            {
                CorrelationId = command.CorrelationId,
                CurrentState = command.CurrentState
            });
        }
    }
}
