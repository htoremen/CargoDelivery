using NoSQLMongo.Domain.Attributes;

namespace NoSQLMongo.Domain.Entities;

[BsonCollection("Cargo")]
public class CargoBson : Document
{
    public CargoBson()
    {
        CargoItems = new HashSet<CargoItemBson>();
    }

    public string CargoId { get; set; }
    public string DebitId { get; set; }
    public string Address { get; set; }

    public virtual DebitBson Debit { get; set; }
    public virtual ICollection<CargoItemBson> CargoItems { get; set; }
}
