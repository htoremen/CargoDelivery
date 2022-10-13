using AutoMapper;
using Cargo.Application.Cargos.CreateDebits;
using Cargo.Application.Telemetry;
using MassTransit.Futures.Contracts;
using System.Diagnostics;

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
        //try
        //{

            var command = context.Message;
            var model = _mapper.Map<CreateDebitCommand>(command);
            var response = await _mediator.Send(model);
        //}
        //catch (Exception ex)
        //{
        //    activity!.SetTag("error.message", ex.Message);
        //}
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