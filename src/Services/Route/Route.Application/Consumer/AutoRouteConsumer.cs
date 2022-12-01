using AutoMapper;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

using Route.Application.Routes.AutoRoutes;
using Route.Application.Routes.StateUpdates;

namespace Route.Application.Consumer;

public class AutoRouteConsumer : IConsumer<IAutoRoute>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IMessageSender<IStartDelivery> _startDelivery;


    public AutoRouteConsumer(IMediator mediator, IMapper mapper, IMessageSender<IStartDelivery> startDelivery)
    {
        _mediator = mediator;
        _mapper = mapper;
        _startDelivery = startDelivery;
    }

    public async Task Consume(ConsumeContext<IAutoRoute> context)
    {
        var command = context.Message;

        var model = _mapper.Map<AutoRouteCommand>(command);
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

public class AutoRouteConsumerDefinition : ConsumerDefinition<AutoRouteConsumer>
{
    public AutoRouteConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AutoRouteConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}