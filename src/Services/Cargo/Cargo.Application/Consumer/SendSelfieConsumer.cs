using Cargo.Application.Cargos.SendSelfie;

namespace Cargo.Application.Consumer;
public class SendSelfieConsumer : IConsumer<ISendSelfie>
{
    private readonly IMediator _mediator;

    public SendSelfieConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ISendSelfie> context)
    {
        var command = context.Message;
        int? maxAttempts = context.Headers.Get("MT-Redelivery-Count", default(int?));

        if (maxAttempts > 3)
        {
            throw new Exception("Something's happened during processing...");
        }

        await _mediator.Send(new SendSelfieCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}