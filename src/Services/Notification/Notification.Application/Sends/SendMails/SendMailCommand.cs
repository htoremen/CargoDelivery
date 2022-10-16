using MediatR;

namespace Notification.Application.Sends.SendMails;

public class SendMailCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class SendMailCommandHandler : IRequestHandler<SendMailCommand>
{
    public async Task<Unit> Handle(SendMailCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}
