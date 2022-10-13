using AutoMapper;
using Cargo.Application.Cargos.SendSelfie;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Cargo.Application.Consumer;
public class SendSelfieConsumer : IConsumer<ISendSelfie>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SendSelfieConsumer(IMediator mediator, IMapper mapper = null)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ISendSelfie> context)
    {
        var command = context.Message;
        var model = _mapper.Map<SendSelfieCommand>(command);

        await _mediator.Send(model);
    }
}

public class SendSelfieConsumerDefinition : ConsumerDefinition<SendSelfieConsumer>
{
    public SendSelfieConsumerDefinition()
    {
        ConcurrentMessageLimit = 3;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SendSelfieConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}