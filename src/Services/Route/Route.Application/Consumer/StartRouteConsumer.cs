using AutoMapper;
using Route.Application.Routes.StartRoutes;
using Route.Application.Routes.StateUpdates;

namespace Route.Application.Consumer;
public class StartRouteConsumer : IConsumer<IStartRoute>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public StartRouteConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IStartRoute> context)
    {
        var command = context.Message;

        var model = _mapper.Map<StartRouteCommand>(command);
        await _mediator.Send(model);

        var state = _mapper.Map<StateUpdateCommand>(command);
        await _mediator.Send(state);
    }
}