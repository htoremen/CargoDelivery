using Delivery.Application.Deliveries.Commands.StartDistributions;

namespace Delivery.Application.Consumer;

public class StartDistributionConsumer : IConsumer<IStartDistribution>
{
    private readonly IMediator _mediator;

    public StartDistributionConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IStartDistribution> context)
    {
        var command = context.Message;
        await _mediator.Send(new StartDistributionCommand { CorrelationId = command.CorrelationId.ToString(), CurrentState = command.CurrentState });
    }
}
