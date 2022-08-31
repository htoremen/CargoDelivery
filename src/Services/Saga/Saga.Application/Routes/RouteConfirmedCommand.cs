namespace Saga.Application.Routes;


public class RouteConfirmedCommand : IRouteConfirmed
{
    public RouteConfirmedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; private set; }

    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }

    public DateTime? SubmitDate { get; set; }
    public DateTime? AcceptDate { get; set; }
}
