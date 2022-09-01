using Route.Application.Routes.ManuelRoutes;

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
        var command = context.Message;

        await _mediator.Send(new ManuelRouteCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}

