using AutoMapper;
using Cargo.Application.Cargos.CreateDebits;
using Cargo.Application.Telemetry;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Cargo.Application.Consumer;
public class CreateDebitConsumer : IConsumer<ICreateDebit>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateDebitConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateDebit> context)
    {
        using var activity = ConsumerActivitySource.Source.StartActivity($"{nameof(CreateDebitConsumer)}");
        activity!.SetTag("CorrelationId", context.Message.CorrelationId);

        var command = context.Message;
        var model = _mapper.Map<CreateDebitCommand>(command);
        var response = await _mediator.Send(model);
    }
}

public class CreateDebitConsumerDefinition : ConsumerDefinition<CreateDebitConsumer>
{
    public CreateDebitConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateDebitConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}