using Core.Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using Payment.Application.Payments.FreeDeliveries;

namespace Payment.Application.Consumer;

public class FreeDeliveryConsumer : IConsumer<IFreeDelivery>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public FreeDeliveryConsumer(IMediator mediator, IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _mediator = mediator;
        _deliveryCompleted = deliveryCompleted;
    }

    public async Task Consume(ConsumeContext<IFreeDelivery> context)
    {
        var command = context.Message;

        await _mediator.Send(new FreeDeliveryCommand
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


public class FreeDeliveryConsumerDefinition : ConsumerDefinition<FreeDeliveryConsumer>
{
    public FreeDeliveryConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FreeDeliveryConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}