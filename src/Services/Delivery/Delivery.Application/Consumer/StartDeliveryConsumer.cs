using Delivery.Application.Deliveries.StartDeliveries;

namespace Delivery.Application.Consumer;

public class StartDeliveryConsumer : IConsumer<IStartDelivery>
{
    private readonly IMediator _mediator;

    public StartDeliveryConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<IStartDelivery> context)
    {
        var command = context.Message;

        await _mediator.Send(new StartDeliveryCommand
        {
            CargoId = command.CargoId,
            CorrelationId = command.CorrelationId,
        });
    }
}
