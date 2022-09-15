using AutoMapper;
using Cargo.Application.Cargos.CreateCargos;

namespace Cargo.Application.Consumer;
public class CreateCargoConsumer : IConsumer<ICreateCargo>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateCargoConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateCargo> context)
    {
        var command = context.Message;
        var model = _mapper.Map<CreateCargoCommand>(command);

        await _mediator.Send(model);

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