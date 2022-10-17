using Deliveries;

namespace Shipment.Application.Shipments.Commands.WasDelivereds;

public class WasDeliveredCommand : IRequest
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}

public class WasDeliveredCommandHandler : IRequestHandler<WasDeliveredCommand>
{
    public async Task<Unit> Handle(WasDeliveredCommand request, CancellationToken cancellationToken)
    {

        return Unit.Value;
    }
}

