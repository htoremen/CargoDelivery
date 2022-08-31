namespace Route.Application.Consumer;

public class AutoRouteConsumer : IConsumer<IAutoRoute>
{
    private readonly IMediator _mediator;

    public AutoRouteConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IAutoRoute> context)
    {

    }
}

