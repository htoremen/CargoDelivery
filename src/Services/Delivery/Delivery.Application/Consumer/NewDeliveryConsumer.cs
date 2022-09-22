using Delivery.Application.Deliveries.Commands.NewDeliveries;

namespace Delivery.Application.Consumer;

public class NewDeliveryConsumer : IConsumer<INewDelivery>
{
    private readonly IMediator _mediator;

    public NewDeliveryConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<INewDelivery> context)
    {
        var command = context.Message;
        await _mediator.Send(new NewDeliveryCommand { CorrelationId = command.CorrelationId.ToString(), CurrentState = command.CurrentState });
    }
}