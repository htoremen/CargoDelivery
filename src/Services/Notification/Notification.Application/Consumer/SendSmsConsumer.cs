using Core.Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using Notification.Application.Sends.SendSmses;


namespace Notification.Application.Consumer;

public class SendSmsConsumer : IConsumer<ISendSms>
{
    private readonly IMediator _mediator;

    public SendSmsConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ISendSms> context)
    {
        var command = context.Message;
        await _mediator.Send(new SendSmsCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId
        });
    }
}

public class SendSmsConsumerDefinition : ConsumerDefinition<SendSmsConsumer>
{
    public SendSmsConsumerDefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SendSmsConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}
