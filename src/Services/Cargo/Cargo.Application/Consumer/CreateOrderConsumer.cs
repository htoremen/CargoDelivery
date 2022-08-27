using Cargo.Application.Cargos.CreateCargos;
using MassTransit;

namespace Cargo.Application.Consumer;
public class CreateOrderConsumer : IConsumer<ICreateOrder>
{
    private readonly IMediator _mediator;

    public CreateOrderConsumer(IMediator mediator)
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
    }
}