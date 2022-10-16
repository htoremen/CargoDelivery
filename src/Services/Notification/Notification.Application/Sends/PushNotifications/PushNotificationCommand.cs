using MediatR;

namespace Notification.Application.Sends.PushNotifications;

public class PushNotificationCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class PushNotificationCommandHandler : IRequestHandler<PushNotificationCommand>
{
    public async Task<Unit> Handle(PushNotificationCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}
