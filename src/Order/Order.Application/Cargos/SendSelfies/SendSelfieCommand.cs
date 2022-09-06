using MassTransit;

namespace Cargo.Application.Cargos.SendSelfies;

public class SendSelfieCommand : IRequest<GenericResponse<SendSelfieResponse>>
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class SendSelfieCommandHandler : IRequestHandler<SendSelfieCommand, GenericResponse<SendSelfieResponse>>
{
    private readonly IMessageSender<ISendSelfie> _sendSelfie;

    public SendSelfieCommandHandler(IMessageSender<ISendSelfie> sendSelfie)
    {
        _sendSelfie = sendSelfie;
    }

    public async Task<GenericResponse<SendSelfieResponse>> Handle(SendSelfieCommand request, CancellationToken cancellationToken)
    {
        await _sendSelfie.SendAsync(new SendSelfie
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);
        var response = new SendSelfieResponse { Id = request.CargoId };

        return GenericResponse<SendSelfieResponse>.Success(response, 200);
    }
}

