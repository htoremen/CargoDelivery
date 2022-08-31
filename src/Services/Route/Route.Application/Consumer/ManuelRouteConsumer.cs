namespace Route.Application.Consumer;

public class ManuelRouteConsumer : IConsumer<IManuelRoute>
{
    private readonly IMediator _mediator;

    public ManuelRouteConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IManuelRoute> context)
    {

    }
}

