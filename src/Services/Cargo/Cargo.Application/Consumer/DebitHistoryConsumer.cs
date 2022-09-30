using AutoMapper;
using Cargo.Application.Cargos.Commands.DebitHistories;
using Cargos;

namespace Cargo.Application.Consumer;

public class DebitHistoryConsumer : IConsumer<IDebitHistory>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DebitHistoryConsumer(IMediator mediator, IMapper mapper = null)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IDebitHistory> context)
    {
        var command = context.Message;
        var debitCommand = _mapper.Map<DebitHistoryCommand>(command);
        await _mediator.Send(debitCommand);
    }
}