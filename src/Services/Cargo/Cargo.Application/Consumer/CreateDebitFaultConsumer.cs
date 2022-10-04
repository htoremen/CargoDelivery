using AutoMapper;
using Cargo.Application.Cargos.Commands.CreateDebitFaults;

namespace Cargo.Application.Consumer;

public class CreateDebitFaultConsumer : IConsumer<Fault<ICreateDebit>>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateDebitFaultConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<Fault<ICreateDebit>> context)
    {
        var command = context.Message.Message;
        var response = await _mediator.Send(new CreateDebitFaultCommand
        {
            CorrelationId = command.CorrelationId,
            CourierId = command.CourierId,
            CurrentState = command.CurrentState,
            DebitId = command.DebitId,
            Cargos = command.Cargos
        });
    }
}
