using Core.Domain.MessageBrokers;

namespace Cargo.Application.Cargos.DebitApprovals;

public class DebitApprovalCommand : IRequest<GenericResponse<DebitApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public bool IsApproved { get; set; }
}

public class DebitApprovalCommandHandler : IRequestHandler<DebitApprovalCommand, GenericResponse<DebitApprovalResponse>>
{
    private readonly IMessageSender<IDebitApproval> _debitApproval;

    public DebitApprovalCommandHandler(IMessageSender<IDebitApproval> DebitApproval)
    {
        _debitApproval = DebitApproval;
    }

    public async Task<GenericResponse<DebitApprovalResponse>> Handle(DebitApprovalCommand request, CancellationToken cancellationToken)
    {
        await _debitApproval.SendAsync(new DebitApproval
        {
            CorrelationId = request.CorrelationId,
            IsApproved = request.IsApproved
        }, null, cancellationToken);
        var response = new DebitApprovalResponse { CorrelationId = request.CorrelationId };

        return GenericResponse<DebitApprovalResponse>.Success(response, 200);
    }
}