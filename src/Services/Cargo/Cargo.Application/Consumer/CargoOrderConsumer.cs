using Cargo.Application.Cargos.CreateCargos;

namespace Cargo.Application.Consumer;
public class CargoOrderConsumer : IConsumer<ICreateOrder>
{
    private readonly IMediator _mediator;

    public CargoOrderConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICreateOrder> context)
    {
        var command = context.Message;

        await _mediator.Send(new CreateCargoCommand
        {
            Id = command.Id,
            CustomerId = command.CustomerId,
            OrderId = command.OrderId,
            ProductId= command.ProductId,
        });

        //await context.Publish<OrderSubmitted>(new
        //{
        //    context.Message.OrderId
        //});
    }
}