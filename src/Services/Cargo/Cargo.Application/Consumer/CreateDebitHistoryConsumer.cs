using AutoMapper;
using Cargo.Application.Cargos.Commands.DebitHistories;
using Cargos;
using System.Text.Json;

namespace Cargo.Application.Consumer;

public class CreateDebitHistoryConsumer : IConsumer<ICreateDebitHistory>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateDebitHistoryConsumer(IMediator mediator, IMapper mapper = null)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateDebitHistory> context)
    {
        var command = context.Message;
        var debitCommand = _mapper.Map<DebitHistoryCommand>(command);
        await _mediator.Send(debitCommand);
    }
}