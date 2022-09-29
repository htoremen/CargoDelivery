namespace Cargos;
public interface ICreateCargo
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CargoId { get; set; }
    public string Address { get; set; }
    public List<CreateCargoCargoItem> CargoItems { get; set; }
}

public class CreateCargo : ICreateCargo
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CargoId { get; set; }
    public string Address { get; set; }
    public List<CreateCargoCargoItem> CargoItems { get; set; }
}

public class CreateCargoCargoItem
{
    public Guid CargoItemId { get; set; }
    public string Barcode { get; set; }
    public string WaybillNumber { get; set; }
    public string Kg { get; set; }
    public string Desi { get; set; }
    public string Description { get; set; }
}