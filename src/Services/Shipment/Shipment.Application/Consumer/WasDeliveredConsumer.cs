using Core.Infrastructure.MessageBrokers.RabbitMQ;

using Shipment.Application.Shipments.Commands.WasDelivereds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipment.Application.Consumer;

public class WasDeliveredConsumer : IConsumer<IWasDelivered>
{
    private readonly IMediator _mediator; 
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public WasDeliveredConsumer(IMediator mediator, IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _mediator = mediator;
        _deliveryCompleted = deliveryCompleted;
    }

    public async Task Consume(ConsumeContext<IWasDelivered> context)
    {
        var command = context.Message;

        await _mediator.Send(new WasDeliveredCommand
        {
            CurrentState = command.CurrentState,
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId
        });

        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CurrentState = command.CurrentState,
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId
        }, null);
    }
}

public class WasDeliveredConsumerDefinition : ConsumerDefinition<WasDeliveredConsumer>
{
    public WasDeliveredConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<WasDeliveredConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}