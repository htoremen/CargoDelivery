namespace Cargo.Application.Cargos.CreateDebits;

public class CreateDebitResponse
{
    public string DebitId { get; set; }
    public Guid CorrelationId { get; set; }
}