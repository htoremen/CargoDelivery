using Core.Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using Notification.Application.Sends.SendMails;


namespace Notification.Application.Consumer;

public class SendMailConsumer : IConsumer<ISendMail>
{
    private readonly IMediator _mediator;

    public SendMailConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ISendMail> context)
    {
        var command = context.Message;
        await _mediator.Send(new SendMailCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId
        });
    }
}

public class SendMailConsumerDefinition : ConsumerDefinition<SendMailConsumer>
{
    public SendMailConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SendMailConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}