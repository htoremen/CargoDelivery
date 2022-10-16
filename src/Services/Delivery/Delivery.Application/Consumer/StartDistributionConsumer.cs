namespace Delivery.Application.Consumer;

public class StartDistributionConsumer : IConsumer<IStartDistribution>
{
    private readonly IMediator _mediator;

    public StartDistributionConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<IStartDistribution> context)
    {
        var command = context.Message;
        throw new NotImplementedException();
    }
}
