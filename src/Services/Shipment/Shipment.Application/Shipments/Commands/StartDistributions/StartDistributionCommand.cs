

namespace Shipment.Application.Shipments.Commands.ShipmentReceiveds;

public class StartDistributionCommand : IRequest
{
    public string CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class StartDistributionCommandHandler : IRequestHandler<StartDistributionCommand>
{
    private readonly IDebitService _debitService;

    public StartDistributionCommandHandler(IDebitService debitService)
    {
        _debitService = debitService;
    }

    public async Task<Unit> Handle(StartDistributionCommand request, CancellationToken cancellationToken)
    {
        await _debitService.UpdateStateAsync(request.CorrelationId, request.CurrentState);
        return Unit.Value;
    }
}