namespace Cargos;

public interface ICreateDebit //: IEvent
{
    public Guid CorrelationId { get; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}

public class CreateDebit : ICreateDebit
{
    public Guid CorrelationId { get; private set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}

public class CreateDebitCargo
{
    public Guid CargoId { get; set; }
    public string Address { get; set; }
    public List<CreateDebitCargoItem> CargoItems { get; set; }
}

public class CreateDebitCargoItem
{
    public Guid CargoItemId { get; set; }
    public string Barcode { get; set; }
    public string WaybillNumber { get; set; }
    public string Kg { get; set; }
    public string Desi { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
}