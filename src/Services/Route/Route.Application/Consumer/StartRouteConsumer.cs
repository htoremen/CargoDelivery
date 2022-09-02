using Route.Application.Routes.StartRoutes;

namespace Route.Application.Consumer;
public class StartRouteConsumer : IConsumer<IStartRoute>
{
    private readonly IMediator _mediator;

    public StartRouteConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IStartRoute> context)
    {
        var command = context.Message;

        await _mediator.Send(new StartRouteCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}
