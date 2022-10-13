using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Delivery.Application.Consumer;

public class ShiftCompletionConsumer : IConsumer<IShiftCompletion>
{
    private readonly IMediator _mediator;

    public ShiftCompletionConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IShiftCompletion> context)
    {
        var command = context.Message;

        //await _mediator.Send(new ShiftCompletionCommand
        //{
        //    CorrelationId = command.CorrelationId
        //});
    }
}

public class ShiftCompletionConsumerDefinition : ConsumerDefinition<ShiftCompletionConsumer>
{
    public ShiftCompletionConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ShiftCompletionConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}