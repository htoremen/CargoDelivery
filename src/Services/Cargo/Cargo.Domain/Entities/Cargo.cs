using System;
using System.Collections.Generic;

namespace Cargo.Domain.Entities
{
    public partial class Cargo
    {
        public Cargo()
        {
            CargoItems = new HashSet<CargoItem>();
        }

        public string CargoId { get; set; } = null!;
        public string? DebitId { get; set; }
        public string? Address { get; set; }

        public virtual Debit? Debit { get; set; }
        public virtual ICollection<CargoItem> CargoItems { get; set; }
    }
}
