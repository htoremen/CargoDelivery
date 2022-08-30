using Core.Domain.Events.Sagas;
using MassTransit;

namespace Order.Application.Consumer;


public class CargoSagaErrorConsumer : IConsumer<ICargoSagaError>
{
    private readonly IMediator _mediator;

    public CargoSagaErrorConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICargoSagaError> context)
    {
        var command = context.Message;
    }
}
