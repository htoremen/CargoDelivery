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

    private readonly ActivitySource _activitySource;

    public CreateDebitConsumer(IMediator mediator, IMapper mapper)
    {
        _activitySource = OpenTelemetryExtensions.CreateActivitySource();
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateDebit> context)
    {
        using var activity = _activitySource.StartActivity($"{nameof(CreateDebitConsumer)}");
        var command = context.Message;
        var model = _mapper.Map<CreateDebitCommand>(command);
        var response = await _mediator.Send(model);
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