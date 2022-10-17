using Core.Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using Payment.Application.Payments.FreeDeliveries;
using Shipments;

namespace Payment.Application.Consumer;

public class FreeDeliveryConsumer : IConsumer<IFreeDelivery>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IWasDelivered> _wasDelivered;

    public FreeDeliveryConsumer(IMediator mediator, IMessageSender<IWasDelivered> wasDelivered)
    {
        _mediator = mediator;
        _wasDelivered = wasDelivered;
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

        await _wasDelivered.SendAsync(new WasDelivered
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