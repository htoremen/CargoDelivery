using AutoMapper;
using Deliveries;
using Route.Application.Routes.ManuelRoutes;
using Route.Application.Routes.StateUpdates;

namespace Route.Application.Consumer;

public class ManuelRouteConsumer : IConsumer<IManuelRoute>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ManuelRouteConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IManuelRoute> context)
    {
        var command = context.Message;

        var model = _mapper.Map<ManuelRouteCommand>(command);
        await _mediator.Send(model);

        var state = _mapper.Map<StateUpdateCommand>(command);
        await _mediator.Send(state);

        await context.Publish<IStartDelivery>(new StartDelivery
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState
        });
    }
}

