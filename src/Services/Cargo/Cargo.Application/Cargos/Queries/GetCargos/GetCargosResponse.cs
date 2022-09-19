namespace Cargo.Application.Cargos.Queries.GetCargos;

public class GetCargosResponse
{
    public string CargoId { get; set; } = null!;
    public string? DebitId { get; set; }
    public string? Address { get; set; }
    public string? Route { get; set; }

    public virtual ICollection<GetCargoItem> CargoItems { get; set; }
}

public partial class GetCargoItem
{
    public string CargoItemId { get; set; } = null!;
    public string CargoId { get; set; } = null!;
    public string Barcode { get; set; } = null!;
    public string? WaybillNumber { get; set; }
    public string? Kg { get; set; }
    public string? Desi { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
}