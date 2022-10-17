using Enums;
using Shipments;

namespace Saga.Application.Deliveries;

public class StartDistributionCommand : IStartDistribution
{
    public StartDistributionCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
    public NotificationType NotificationType { get; set; }
}
