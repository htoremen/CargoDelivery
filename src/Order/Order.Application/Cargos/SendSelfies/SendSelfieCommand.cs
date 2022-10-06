using MassTransit;

namespace Cargo.Application.Cargos.SendSelfies;

public class SendSelfieCommand : IRequest<GenericResponse<SendSelfieResponse>>
{
    public Guid CorrelationId { get; set; }
}

public class SendSelfieCommandHandler : IRequestHandler<SendSelfieCommand, GenericResponse<SendSelfieResponse>>
{
    private readonly IMessageSender<ISendSelfie> _sendSelfie;
    private readonly IRequestClient<ISendSelfie> _sendSelfieClient;

    public SendSelfieCommandHandler(IMessageSender<ISendSelfie> sendSelfie, IRequestClient<ISendSelfie> sendSelfieClient)
    {
        _sendSelfie = sendSelfie;
        _sendSelfieClient = sendSelfieClient;
    }

    public async Task<GenericResponse<SendSelfieResponse>> Handle(SendSelfieCommand request, CancellationToken cancellationToken)
    {
        //var data = await _sendSelfieClient.GetResponse<ISendSelfie>(new { CorrelationId = request.CorrelationId });

        await _sendSelfie.SendAsync(new SendSelfie
        {
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        var response = new SendSelfieResponse { CorrelationId = request.CorrelationId };

        return GenericResponse<SendSelfieResponse>.Success(response, 200);
    }
}

