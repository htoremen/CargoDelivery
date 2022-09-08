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
        await _mediator.Send(new SendSelfieCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });
    }
}