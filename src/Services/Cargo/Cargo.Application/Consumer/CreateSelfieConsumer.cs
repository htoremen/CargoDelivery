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
        int? maxAttempts = context.Headers.Get("MT-Redelivery-Count", default(int?));

        if (maxAttempts > 3)
        {
            throw new Exception("Something's happened during processing...");
        }

        await _mediator.Send(new CreateSelfieCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}