namespace Cargos;

public interface ICreateCargo //: IEvent
{
    public Guid CorrelationId { get; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CargoDetay> Cargos { get; set; }
}

public class CreateCargo : ICreateCargo
{
    public Guid CorrelationId { get; private set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CargoDetay> Cargos { get; set; }
}

public class CargoDetay
{
    public string Address { get; set; }
    public List<CreateCargoCargoItem> CargoItems { get; set; }
}

public class CreateCargoCargoItem
{
    public string Barcode { get; set; }
    public string WaybillNumber { get; set; }
    public string Kg { get; set; }
    public string Desi { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
}