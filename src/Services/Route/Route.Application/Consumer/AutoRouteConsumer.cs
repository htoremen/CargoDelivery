using AutoMapper;
using Route.Application.Routes.AutoRoutes;
using Route.Application.Routes.StateUpdates;

namespace Route.Application.Consumer;

public class AutoRouteConsumer : IConsumer<IAutoRoute>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AutoRouteConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IAutoRoute> context)
    {
        var command = context.Message;

        var model = _mapper.Map<AutoRouteCommand>(command);
        await _mediator.Send(model);

        var state = _mapper.Map<StateUpdateCommand>(command);
        await _mediator.Send(state);
    }
}

