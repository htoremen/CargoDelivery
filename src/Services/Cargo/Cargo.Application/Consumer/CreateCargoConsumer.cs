using Cargo.Application.Cargos.CreateCargos;

namespace Cargo.Application.Consumer;
public class CreateCargoConsumer : IConsumer<ICreateCargo>
{
    private readonly IMediator _mediator;

    public CreateCargoConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICreateCargo> context)
    {
        var command = context.Message;

        await _mediator.Send(new CreateCargoCommand
        {
            CorrelationId = command.CorrelationId,
            DebitId = command.DebitId,
            CourierId = command.CourierId,
            Cargos = command.Cargos
        });

        //await context.Publish<OrderSubmitted>(new
        //{
        //    context.Message.OrderId
        //});
    }
}

//public class CreateCargoFaultConsumer : IConsumer<Fault<CreateCargo>>
//{
//    public async Task Consume(ConsumeContext<Fault<CreateCargo>> context)
//    {
//        var command = context.Message;

//        //await context.Publish<OrderSubmitted>(new
//        //{
//        //    context.Message.OrderId
//        //});
//    }
//}


//public class CreateCargoConsumerDefinition : ConsumerDefinition<CreateCargoConsumer>
//{
//    public CreateCargoConsumerDefinition()
//    {
//        // override the default endpoint name, for whatever reason
//       // EndpointName = "create-cargo";

//        // limit the number of messages consumed concurrently
//        // this applies to the consumer only, not the endpoint
//        ConcurrentMessageLimit = 4;
//    }

//    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateCargoConsumer> consumerConfigurator)
//    {
//        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
//        endpointConfigurator.UseInMemoryOutbox();
//    }
//}