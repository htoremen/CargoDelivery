using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Shipment.Application.Shipments.Commands.ShipmentReceiveds;

namespace Shipment.Application.Consumer;

public class StartDistributionConsumer : IConsumer<IStartDistribution>
{
    private readonly IMediator _mediator;
    private readonly IMessageSender<ISendMail> _sendmail;
    private readonly IMessageSender<ISendSms> _sendSms;
    private readonly IMessageSender<IPushNotification> _pushNotification;

    public StartDistributionConsumer(IMediator mediator, IMessageSender<ISendSms> sendSms, IMessageSender<ISendMail> sendmail, IMessageSender<IPushNotification> pushNotification)
    {
        _mediator = mediator;
        _sendmail = sendmail;
        _sendSms = sendSms;
        _pushNotification = pushNotification;
    }

    public async Task Consume(ConsumeContext<IStartDistribution> context)
    {
        var command = context.Message;
        await _mediator.Send(new StartDistributionCommand { CorrelationId = command.CorrelationId.ToString(), CurrentState = command.CurrentState });
       
        using var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        if (command.NotificationType == NotificationType.SendMail)
        {
            await _sendmail.SendAsync(new SendMail
            {
                CorrelationId = command.CorrelationId,
                CargoId = command.CargoId,
                CurrentState = command.CurrentState
            });
        }
        else if (command.NotificationType == NotificationType.SendSms)
        {
            await _sendSms.SendAsync(new SendSms
            {
                CorrelationId = command.CorrelationId,
                CargoId = command.CargoId,
                CurrentState = command.CurrentState
            });
        }
        else if (command.NotificationType == NotificationType.PushNotification)
        {
            await _pushNotification.SendAsync(new PushNotification
            {
                CorrelationId = command.CorrelationId,
                CargoId = command.CargoId,
                CurrentState = command.CurrentState
            });
        }
    }
}


public class StartDistributionConsumerDefinition : ConsumerDefinition<StartDistributionConsumer>
{
    public StartDistributionConsumerDefinition()
    {
        ConcurrentMessageLimit = 3;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<StartDistributionConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}