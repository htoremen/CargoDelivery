using NoSQLMongo.Domain.Attributes;

namespace NoSQLMongo.Domain.Entities;

[BsonCollection("Debit")]
public class DebitBson : Document
{
    public DebitBson()
    {
        Cargos = new List<CargoBson>();
    }

    public string DebitId { get; set; }
    public string CourierId { get; set; }
    public string CorrelationId { get; set; }
    public string ApprovingId { get; set; }
    public string Selfie { get; set; }
    public bool? IsApproval { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime DistributionDate { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime? ClosingDate { get; set; }

    public virtual ICollection<CargoBson> Cargos { get; set; }
}