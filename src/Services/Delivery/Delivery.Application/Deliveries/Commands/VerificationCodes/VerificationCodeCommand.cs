namespace Delivery.Application.Deliveries.Commands.VerificationCodes;

public class VerificationCodeCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public int Code { get; set; }
}
public class VerificationCodeCommandHandler : IRequestHandler<VerificationCodeCommand>
{
    public async Task<Unit> Handle(VerificationCodeCommand request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}

