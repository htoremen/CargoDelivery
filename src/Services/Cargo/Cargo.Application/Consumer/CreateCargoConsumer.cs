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
            Id = command.Id,
            CustomerId = command.CustomerId,
            CargoId = command.CargoId,
            ProductId= command.ProductId,
        });

        //await context.Publish<OrderSubmitted>(new
        //{
        //    context.Message.OrderId
        //});
    }
}