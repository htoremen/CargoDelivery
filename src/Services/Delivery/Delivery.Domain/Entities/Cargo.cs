using System;
using System.Collections.Generic;

namespace Delivery.Domain.Entities
{
    public partial class Cargo
    {
        public Cargo()
        {
            CargoItems = new HashSet<CargoItem>();
            Deliveries = new HashSet<Delivery>();
        }

        public string CargoId { get; set; } = null!;
        public string? DebitId { get; set; }
        public string? Address { get; set; }
        public string? Route { get; set; }

        public virtual ICollection<CargoItem> CargoItems { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}
