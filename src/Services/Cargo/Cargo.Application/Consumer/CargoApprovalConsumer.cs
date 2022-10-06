using Cargo.Application.Cargos.CargoApprovals;

namespace Cargo.Application.Consumer;

public class CargoApprovalConsumer : IConsumer<ICargoApproval>
{
    private readonly IMediator _mediator;

    public CargoApprovalConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICargoApproval> context)
    {
        var command = context.Message;

        await _mediator.Send(new CargoApprovalCommand
        {
            CorrelationId = command.CorrelationId,
            CurrentState = command.CurrentState,
        });
    }
}