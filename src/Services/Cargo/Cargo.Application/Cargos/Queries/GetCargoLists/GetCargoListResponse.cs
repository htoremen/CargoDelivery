namespace Cargo.Application.Cargos.Queries.GetCargoLists;

public class GetCargoListResponse
{
    public string CargoId { get; set; } = null!;
    public string? DebitId { get; set; }
    public string? Address { get; set; }
}
