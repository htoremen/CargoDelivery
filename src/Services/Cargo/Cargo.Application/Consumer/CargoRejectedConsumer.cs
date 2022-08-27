using Cargo.Application.Cargos.CargoRejecteds;

namespace Cargo.Application.Consumer;

public class CargoRejectedConsumer : IConsumer<ICargoRejected>
{
    private readonly IMediator _mediator;

    public CargoRejectedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICargoRejected> context)
    {
        var command = context.Message;

        await _mediator.Send(new CargoRejectedCommand
        {
            CorrelationId = command.CorrelationId,
        });

    }
}

