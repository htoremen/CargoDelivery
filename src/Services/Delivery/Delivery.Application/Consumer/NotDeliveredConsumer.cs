using Delivery.Application.Deliveries.NotDelivereds;

namespace Delivery.Application.Consumer;

public class NotDeliveredConsumer : IConsumer<INotDelivered>
{
    private readonly IMediator _mediator;

    public NotDeliveredConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<INotDelivered> context)
    {
        var command = context.Message;

        await _mediator.Send(new NotDeliveredCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}
