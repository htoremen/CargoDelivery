using Delivery.Application.Deliveries.CreateRefunds;

namespace Delivery.Application.Consumer;

public class CreateRefundConsumer : IConsumer<ICreateRefund>
{
    private readonly IMediator _mediator;

    public CreateRefundConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICreateRefund> context)
    {
        var command = context.Message;

        await _mediator.Send(new CreateRefundCommand
        {
            CorrelationId = command.CorrelationId,
        });
    }
}

