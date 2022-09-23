using MassTransit;
using Payment.Application.Deliveries.Commands.UpdatePaymentTypes;
using Payment.Application.Payments.CardPayments;

namespace Payment.Application.Consumer;

public class CardPaymentConsumer : IConsumer<ICardPayment>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<IDeliveryCompleted> _deliveryCompleted;

    public CardPaymentConsumer(IMediator mediator, IMessageSender<IDeliveryCompleted> deliveryCompleted)
    {
        _mediator = mediator;
        _deliveryCompleted = deliveryCompleted;
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

        await _deliveryCompleted.SendAsync(new DeliveryCompleted
        {
            CurrentState = command.CurrentState,
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId
        }, null);
    }
}
