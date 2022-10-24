using AutoMapper;
using Cargos;
using Core.Domain.MessageBrokers;
using Shipments;
using static Cargos.IDebitRejected;

namespace Cargo.Application.Cargos.DebitApprovals;

public class DebitApprovalCommand : IRequest<GenericResponse<DebitApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public bool IsApproved { get; set; }
}

public class DebitApprovalCommandHandler : IRequestHandler<DebitApprovalCommand, GenericResponse<DebitApprovalResponse>>
{
    private readonly IMessageSender<IShipmentReceived> _shipmentReceived;
    private readonly IMessageSender<IDebitRejected> _DebitRejected;
    private readonly IMapper _mapper;

    public DebitApprovalCommandHandler(IMessageSender<IShipmentReceived> shipmentReceived, IMessageSender<IDebitRejected> DebitRejected, IMapper mapper)
    {
        _shipmentReceived = shipmentReceived;
        _DebitRejected = DebitRejected;
        _mapper = mapper;
    }

    public async Task<GenericResponse<DebitApprovalResponse>> Handle(DebitApprovalCommand request, CancellationToken cancellationToken)
    {
        if (request.IsApproved)
        {
            await _shipmentReceived.SendAsync(new ShipmentReceived
            {
                CorrelationId = request.CorrelationId,
                CurrentState = request.CurrentState,
            }, null, cancellationToken);
        }
        else
        {
            await _DebitRejected.SendAsync(new DebitRejected
            {
                CorrelationId = request.CorrelationId,
                CurrentState = request.CurrentState,
            }, null, cancellationToken);
        }
        return GenericResponse<DebitApprovalResponse>.Success(new DebitApprovalResponse { }, 200);
    }
}