using MassTransit;
using Payment.Application.Payments.FreeDeliveries;

namespace Payment.Application.Consumer;

public class FreeDeliveryConsumer : IConsumer<IFreeDelivery>
{
    private readonly IMediator _mediator;

    public FreeDeliveryConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IFreeDelivery> context)
    {
        var command = context.Message;

        await _mediator.Send(new FreeDeliveryCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}