namespace Order.Application.Deliveries.VerificationCodes;

public class VerificationCodeCommand : IRequest<Unit>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public int Code { get; set; }
}

public class VerificationCodeCommandHandler : IRequestHandler<VerificationCodeCommand, Unit>
{
    private readonly IMessageSender<IVerificationCode> _verificationCode;

    public VerificationCodeCommandHandler(IMessageSender<IVerificationCode> verificationCode)
    {
        _verificationCode = verificationCode;
    }

    public async Task<Unit> Handle(VerificationCodeCommand request, CancellationToken cancellationToken)
    {
        await _verificationCode.SendAsync(new VerificationCode
        {
            CorrelationId = request.CorrelationId,
            CargoId = request.CargoId,
            Code = request.Code
        });
        return Unit.Value;

    }
}