using MassTransit;
using Payment.Application.Payments.CardPayments;

namespace Payment.Application.Consumer;

public class CardPaymentConsumer : IConsumer<ICardPayment>
{
    private readonly IMediator _mediator;

    public CardPaymentConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICardPayment> context)
    {
        var command = context.Message;

        await _mediator.Send(new CardPaymentCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}
