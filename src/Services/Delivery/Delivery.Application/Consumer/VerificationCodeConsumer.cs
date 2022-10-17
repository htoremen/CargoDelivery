using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Delivery.Application.Deliveries.Commands.VerificationCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Application.Consumer;

public class VerificationCodeConsumer : IConsumer<IVerificationCode>
{
    private readonly IMediator _mediator;

    public VerificationCodeConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<IVerificationCode> context)
    {
        var command = context.Message;

        await _mediator.Send(new VerificationCodeCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
            Code = command.Code
        });
    }
}


public class VerificationCodeConsumerefinition : ConsumerDefinition<VerificationCodeConsumer>
{
    public VerificationCodeConsumerefinition()
    {
        ConcurrentMessageLimit = SetConfigureConsumer.ConcurrentMessageLimit();
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<VerificationCodeConsumer> consumerConfigurator)
    {
        endpointConfigurator.SetConfigure();
    }
}