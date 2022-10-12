using AutoMapper;
using Cargo.Application.Cargos.CreateDebits;
using Core.Domain.MessageBrokers;

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
        // new NullReferenceException("Debit object is null.");
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

public class CreateDebitConsumerDefinition : ConsumerDefinition<CreateDebitConsumer>
{
    public CreateDebitConsumerDefinition()
    {
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateDebitConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}