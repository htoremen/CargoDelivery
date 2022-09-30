using System;
using System.Collections.Generic;

namespace Cargo.Domain.Entities
{
    public partial class Debit
    {
        public Debit()
        {
            Cargos = new HashSet<Cargo>();
        }

        public string DebitId { get; set; }
        public string CourierId { get; set; }
        public string CorrelationId { get; set; }
        public string ApprovingId { get; set; }
        public string CurrentState { get; set; }
        public string Selfie { get; set; }
        public DateTime DistributionDate { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool? IsApproval { get; set; }
        public bool? IsCompleted { get; set; }

        public virtual ICollection<Cargo> Cargos { get; set; }
    }
}
