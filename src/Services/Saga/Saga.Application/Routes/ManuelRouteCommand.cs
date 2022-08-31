namespace Saga.Application.Routes;

public class ManuelRouteCommand : IManuelRoute
{
    public ManuelRouteCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}
