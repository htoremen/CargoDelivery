﻿using Delivery.Application.Deliveries.DeliveryCompleteds;

namespace Delivery.Application.Consumer;
public class DeliveryCompletedConsumer : IConsumer<IDeliveryCompleted>
{
    private readonly IMediator _mediator;

    public DeliveryCompletedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<IDeliveryCompleted> context)
    {
        var command = context.Message;

        await _mediator.Send(new DeliveryCompletedCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}