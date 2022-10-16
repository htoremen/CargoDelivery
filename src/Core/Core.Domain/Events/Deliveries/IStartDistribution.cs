namespace Deliveries;

public interface IStartDistribution
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
public class StartDistribution : IStartDistribution
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CargoId { get; set; }
}