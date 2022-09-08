namespace Cargo.Application.Cargos.CreateCargos;

public class CargoItemRequest
{
    public string Barcode { get; set; }
    public string WaybillNumber { get; set; }
    public string Kg { get; set; }
    public string Desi { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
}
