using Core.Domain.Events.Deliveries;

namespace Delivery.Application.Consumer;

public class NewDeliveryConsumer : IConsumer<INewDelivery>
{
    private readonly IMediator _mediator;

    public NewDeliveryConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<INewDelivery> context)
    {
        var command = context.Message;

    }
}

