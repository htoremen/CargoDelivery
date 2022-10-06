using Delivery.Application.Deliveries.Commands.InsertDeliveries;
using Delivery.Application.Deliveries.CreateDeliveries;

namespace Delivery.Application.Consumer;

public class CreateDeliveryConsumer : IConsumer<ICreateDelivery>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<ICardPayment> _cardPayment;
    private readonly IMessageSender<IPayAtDoor> _payAtDoor;
    private readonly IMessageSender<IFreeDelivery> _freeDelivery;

    public CreateDeliveryConsumer(IMediator mediator, IMessageSender<ICardPayment> cardPayment, IMessageSender<IPayAtDoor> payAtDoor, IMessageSender<IFreeDelivery> freeDelivery)
    {
        _mediator = mediator;

        _cardPayment = cardPayment;
        _payAtDoor = payAtDoor;
        _freeDelivery = freeDelivery;
    }

    public async Task Consume(ConsumeContext<ICreateDelivery> context)
    {
        var command = context.Message;

        await _mediator.Send(new InsertDeliveryCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
            CurrentState = command.CurrentState,
            DeliveryType = DeliveryType.CreateDelivery
        });

        //await _mediator.Send(new CreateDeliveryCommand
        //{
        //    CorrelationId = command.CorrelationId,
        //    CargoId = command.CargoId,
        //    PaymentType = command.PaymentType,
        //});


        if (command.PaymentType == PaymentType.CardPayment)
        {
            await _cardPayment.SendAsync(new CardPayment
            {
                CargoId = command.CargoId,
                CorrelationId = command.CorrelationId,
                PaymentType = command.PaymentType
            }, null);
        }
        else if (command.PaymentType == PaymentType.PayAtDoor)
        {
            await _payAtDoor.SendAsync(new PayAtDoor
            {
                CargoId = command.CargoId,
                CorrelationId = command.CorrelationId,
                PaymentType = command.PaymentType
            }, null);
        }
        else if (command.PaymentType == PaymentType.FreeDelivery)
        {
            await _freeDelivery.SendAsync(new FreeDelivery
            {
                CargoId = command.CargoId,
                CorrelationId = command.CorrelationId,
                PaymentType = command.PaymentType
            }, null);
        }
    }
}