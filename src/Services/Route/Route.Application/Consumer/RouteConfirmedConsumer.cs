namespace Route.Application.Consumer;

public class RouteConfirmedConsumer : IConsumer<IRouteConfirmed>
{
    private readonly IMediator _mediator;

    public RouteConfirmedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IRouteConfirmed> context)
    {
        var command = context.Message;
    }
}

