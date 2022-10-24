using AutoMapper;
using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Deliveries;
using Route.Application.Routes.ManuelRoutes;
using Route.Application.Routes.StateUpdates;

namespace Route.Application.Consumer;

public class ManuelRouteConsumer : IConsumer<IManuelRoute>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IMessageSender<IStartDelivery> _startDelivery;

    public ManuelRouteConsumer(IMediator mediator, IMapper mapper, IMessageSender<IStartDelivery> startDelivery)
    {
        _mediator = mediator;
        _mapper = mapper;
        _startDelivery = startDelivery;
    }

    public async Task Consume(ConsumeContext<IManuelRoute> context)
    {
        var command = context.Message;

        var model = _mapper.Map<ManuelRouteCommand>(command);
        await _mediator.Send(model);

        var state = _mapper.Map<StateUpdateCommand>(command);
        await _mediator.Send(state);

        await _startDelivery.SendAsync(new StartDelivery
        {
            CurrentState = command.CurrentState,
            CorrelationId = command.CorrelationId
        }, null);
    }
}


public class ManuelRouteConsumerDefinition : ConsumerDefinition<ManuelRouteConsumer>
{
    public ManuelRouteConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ManuelRouteConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}
