using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Delivery.Application.Deliveries.Commands.InsertDeliveries;
using Delivery.Application.Deliveries.CreateRefunds;

namespace Delivery.Application.Consumer;

public class CreateRefundConsumer : IConsumer<ICreateRefund>
{
    private readonly IMediator _mediator; 
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public CreateRefundConsumer(IMediator mediator, IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _mediator = mediator;
        _deliveryCompleted = deliveryCompleted;
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

        await _deliveryCompleted.SendAsync(new DeliveryCompleted
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