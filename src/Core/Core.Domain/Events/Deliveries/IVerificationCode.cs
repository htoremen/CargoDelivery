namespace Deliveries;

public interface IVerificationCode
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public int Code { get; set; }
}
public class VerificationCode : IVerificationCode
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public int Code { get; set; }
}