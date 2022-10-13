using Core.Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using Payment.Application.Payments.PayAtDoors;

namespace Payment.Application.Consumer;

public class PayAtDoorConsumer : IConsumer<IPayAtDoor>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public PayAtDoorConsumer(IMediator mediator, IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _mediator = mediator;
        _deliveryCompleted = deliveryCompleted;
    }

    public async Task Consume(ConsumeContext<IPayAtDoor> context)
    {
        var command = context.Message;

        await _mediator.Send(new PayAtDoorCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });

        await _mediator.Send(new UpdatePaymentTypeCommand
        {
            CorrelationId = command.CorrelationId.ToString(),
            CargoId = command.CargoId.ToString(),
            PaymentType = (int)command.PaymentType
        });

        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CurrentState = command.CurrentState,
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId
        }, null);
    }
}

public class PayAtDoorConsumerDefinition : ConsumerDefinition<PayAtDoorConsumer>
{
    public PayAtDoorConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PayAtDoorConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}