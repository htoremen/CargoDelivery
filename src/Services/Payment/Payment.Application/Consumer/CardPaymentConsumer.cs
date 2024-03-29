﻿using Core.Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using Payment.Application.Deliveries.Commands.UpdatePaymentTypes;
using Payment.Application.Payments.CardPayments;


namespace Payment.Application.Consumer;

public class CardPaymentConsumer : IConsumer<ICardPayment>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IWasDelivered> _wasDelivered;

    public CardPaymentConsumer(IMediator mediator, IMessageSender<IWasDelivered> wasDelivered)
    {
        _mediator = mediator;
        _wasDelivered = wasDelivered;
    }

    public async Task Consume(ConsumeContext<ICardPayment> context)
    {
        var command = context.Message;

        await _mediator.Send(new CardPaymentCommand
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

public class CardPaymentConsumerDefinition : ConsumerDefinition<CardPaymentConsumer>
{
    public CardPaymentConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CardPaymentConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}