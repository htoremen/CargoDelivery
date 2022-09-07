using Cargo.Application.Cargos.SendSelfie;

namespace Cargo.Application.Consumer;
public class SendSelfieConsumer : IConsumer<ISendSelfie>
{
    private readonly IMediator _mediator;
    private readonly IRequestClient<SendSelfie> _client;

    public SendSelfieConsumer(IMediator mediator, IRequestClient<SendSelfie> client)
    {
        _mediator = mediator;
        _client = client;
    }

    public async Task Consume(ConsumeContext<ISendSelfie> context)
    {
        var command = context.Message;
       // Response<SendSelfie> response = await _client.GetResponse<SendSelfie>(new { command.CorrelationId });

        await _mediator.Send(new SendSelfieCommand
        {
            CorrelationId = command.CorrelationId,
            CargoId = command.CargoId,
        });

       // return context.RespondAsync(command);
    }
}