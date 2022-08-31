namespace Routes;
public interface IRouteConfirmed //: IEvent
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
    public DateTime? SubmitDate { get; set; }
    public DateTime? AcceptDate { get; set; }
}


public class RouteConfirmed : IRouteConfirmed
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
    public DateTime? SubmitDate { get; set; }
    public DateTime? AcceptDate { get; set; }
}