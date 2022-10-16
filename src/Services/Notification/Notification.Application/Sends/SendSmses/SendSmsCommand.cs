using MediatR;

namespace Notification.Application.Sends.SendSmses;

public class SendSmsCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
public class SendSmsCommandHandler : IRequestHandler<SendSmsCommand>
{
    public async Task<Unit> Handle(SendSmsCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}
