using AutoMapper;
using Cargo.Application.Cargos.Commands.CreateCargos;
using Cargo.Application.Cargos.CreateDebits;
using Core.Domain.MessageBrokers;
using System.Text.Json;

namespace Cargo.Application.Consumer;
public class CreateDebitConsumer : IConsumer<ICreateDebit>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IMessageSender<ICreateDebitHistory> _debitHistory;

    public CreateDebitConsumer(IMediator mediator, IMapper mapper, IMessageSender<ICreateDebitHistory> debitHistory)
    {
        _mediator = mediator;
        _mapper = mapper;
        _debitHistory = debitHistory;
    }

    public async Task Consume(ConsumeContext<ICreateDebit> context)
    {
        var command = context.Message;
        var model = _mapper.Map<CreateDebitCommand>(command);
        var response = await _mediator.Send(model);

        //await _debitHistory.SendAsync(new CreateDebitHistory
        //{
        //    DebitId = command.DebitId.ToString(),
        //    CommandName = "ICreateDebit",
        //    CourierId = command.CourierId.ToString(),
        //    //Request = JsonSerializer.Serialize(model), 
        //    //Response = JsonSerializer.Serialize(response)
        //});
    }
}