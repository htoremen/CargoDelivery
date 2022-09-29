namespace Saga.Application.Cargos;

public class CreateDebitCommand : ICreateDebit
{
    public CreateDebitCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; }
    public string CurrentState { get; set; }

    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}