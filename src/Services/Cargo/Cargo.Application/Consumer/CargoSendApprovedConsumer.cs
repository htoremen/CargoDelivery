using Cargo.Application.Cargos.CargoRejecteds;
using Cargo.Application.Cargos.CargoSendApproveds;

namespace Cargo.Application.Consumer;
public class CargoSendApprovedConsumer : IConsumer<ICargoSendApproved>
{
    private readonly IMediator _mediator;

    public CargoSendApprovedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICargoSendApproved> context)
    {
        var command = context.Message;

        await _mediator.Send(new CargoSendApprovedCommand
        {
            Id = command.CorrelationId,
        });
    }
}
