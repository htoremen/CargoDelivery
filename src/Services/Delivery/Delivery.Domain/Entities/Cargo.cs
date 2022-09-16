using System;
using System.Collections.Generic;

namespace Delivery.Domain.Entities
{
    public partial class Cargo
    {
        public Cargo()
        {
            CargoItems = new HashSet<CargoItem>();
        }

        public string CargoId { get; set; } = null!;
        public string? DeliveryId { get; set; }

        public virtual Delivery? Delivery { get; set; }
        public virtual ICollection<CargoItem> CargoItems { get; set; }
    }
}
