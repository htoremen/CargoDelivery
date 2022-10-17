using Cargos;

namespace Core.Domain.SerializerModels;

public class CreateDebitModel
{
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}
