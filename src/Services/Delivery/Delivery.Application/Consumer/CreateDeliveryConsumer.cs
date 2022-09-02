using Delivery.Application.Deliveries.CreateDeliveries;

namespace Delivery.Application.Consumer;

public class CreateDeliveryConsumer : IConsumer<ICreateDelivery>
{
    private readonly IMediator _mediator;

    public CreateDeliveryConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICreateDelivery> context)
    {
        var command = context.Message;

        await _mediator.Send(new CreateDeliveryCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}

