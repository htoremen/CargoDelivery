using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Delivery.Application.Deliveries.Commands.InsertDeliveries;
using Delivery.Application.Deliveries.NotDelivereds;
using Shipments;

namespace Delivery.Application.Consumer;

public class NotDeliveredConsumer : IConsumer<INotDelivered>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IWasDelivered> _wasDelivered;

    public NotDeliveredConsumer(IMediator mediator, IMessageSender<IWasDelivered> wasDelivered)
    {
        _mediator = mediator;
        _wasDelivered = wasDelivered;
    }
    public async Task Consume(ConsumeContext<INotDelivered> context)
    {
        var command = context.Message;

        await _mediator.Send(new InsertDeliveryCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
            CurrentState = command.CurrentState,
            DeliveryType = DeliveryType.NotDelivered
        });

        await _mediator.Send(new NotDeliveredCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
            CurrentState = command.CurrentState,
            DeliveryType = DeliveryType.NotDelivered
        });

        await _wasDelivered.SendAsync(new WasDelivered
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState,
            CargoId = command.CargoId,
        }, null);
    }
}

public class NotDeliveredConsumerDefinition : ConsumerDefinition<NotDeliveredConsumer>
{
    public NotDeliveredConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<NotDeliveredConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}