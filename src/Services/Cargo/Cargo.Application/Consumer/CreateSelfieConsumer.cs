using Cargo.Application.Cargos.CreateSelfies;

namespace Cargo.Application.Consumer;
public class CreateSelfieConsumer : IConsumer<ICreateSelfie>
{
    private readonly IMediator _mediator;

    public CreateSelfieConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ICreateSelfie> context)
    {
        var command = context.Message;

        await _mediator.Send(new CreateSelfieCommand
        {
            Id = command.Id,
        });
    }
}