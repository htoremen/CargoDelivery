using MassTransit;
using Payment.Application.Payments.PayAtDoors;

namespace Payment.Application.Consumer;

public class PayAtDoorConsumer : IConsumer<IPayAtDoor>
{
    private readonly IMediator _mediator;

    public PayAtDoorConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IPayAtDoor> context)
    {
        var command = context.Message;

        await _mediator.Send(new PayAtDoorCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}