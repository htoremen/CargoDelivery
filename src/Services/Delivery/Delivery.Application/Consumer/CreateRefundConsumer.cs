using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Delivery.Application.Deliveries.Commands.InsertDeliveries;
using Delivery.Application.Deliveries.CreateRefunds;
using Shipments;

namespace Delivery.Application.Consumer;

public class CreateRefundConsumer : IConsumer<ICreateRefund>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IWasDelivered> _wasDelivered;

    public CreateRefundConsumer(IMediator mediator, IMessageSender<IWasDelivered> wasDelivered)
    {
        _mediator = mediator;
        _wasDelivered = wasDelivered;
    }

    public async Task Consume(ConsumeContext<ICreateRefund> context)
    {
        var command = context.Message;

        await _mediator.Send(new InsertDeliveryCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
            CurrentState = command.CurrentState,
            DeliveryType = DeliveryType.CreateRefund
        });

        await _mediator.Send(new CreateRefundCommand
        {
            CorrelationId = command.CorrelationId,
        });

        await _wasDelivered.SendAsync(new WasDelivered
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState,
            CargoId = command.CargoId,
        }, null);
    }
}

public class CreateRefundConsumerDefinition : ConsumerDefinition<CreateRefundConsumer>
{
    public CreateRefundConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateRefundConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}