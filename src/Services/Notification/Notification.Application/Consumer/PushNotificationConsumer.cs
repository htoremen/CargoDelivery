﻿using Core.Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using MassTransit.Mediator;
using Notification.Application.Sends.PushNotifications;
using Notifications;

namespace Notification.Application.Consumer;

public class PushNotificationConsumer : IConsumer<IPushNotification>
{
    private readonly IMediator _mediator;

    public PushNotificationConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IPushNotification> context)
    {
        var command = context.Message;
        await _mediator.Send(new PushNotificationCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId
        });
    }
}

public class PushNotificationConsumerDefinition : ConsumerDefinition<PushNotificationConsumer>
{
    public PushNotificationConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateRefundConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}
