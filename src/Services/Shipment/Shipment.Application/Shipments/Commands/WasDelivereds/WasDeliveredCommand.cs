

namespace Shipment.Application.Shipments.Commands.WasDelivereds;

public class WasDeliveredCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}

public class WasDeliveredCommandHandler : IRequestHandler<WasDeliveredCommand>
{
    private readonly IDebitService _debitService;

    public WasDeliveredCommandHandler(IDebitService debitService)
    {
        _debitService = debitService;
    }
    public async Task<Unit> Handle(WasDeliveredCommand request, CancellationToken cancellationToken)
    {
        await _debitService.UpdateStateAsync(request.CorrelationId.ToString(), request.CurrentState);
        return Unit.Value;
    }
}

