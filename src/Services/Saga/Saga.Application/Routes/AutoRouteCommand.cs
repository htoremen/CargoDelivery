namespace Saga.Application.Routes;
public class AutoRouteCommand : IAutoRoute
{
    public AutoRouteCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

