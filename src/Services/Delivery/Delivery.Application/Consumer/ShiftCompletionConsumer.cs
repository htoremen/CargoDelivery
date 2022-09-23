using Delivery.Application.Deliveries.ShiftCompletions;

namespace Delivery.Application.Consumer;

public class ShiftCompletionConsumer : IConsumer<IShiftCompletion>
{
    private readonly IMediator _mediator;

    public ShiftCompletionConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IShiftCompletion> context)
    {
        var command = context.Message;

        await _mediator.Send(new ShiftCompletionCommand
        {
            CorrelationId = command.CorrelationId
        });
    }
}
