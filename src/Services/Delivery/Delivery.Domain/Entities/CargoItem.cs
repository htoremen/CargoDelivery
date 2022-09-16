using System;
using System.Collections.Generic;

namespace Delivery.Domain.Entities
{
    public partial class CargoItem
    {
        public string CargoItemId { get; set; } = null!;
        public string? CargoId { get; set; }
        public bool? IsWasDelivered { get; set; }

        public virtual Cargo? Cargo { get; set; }
    }
}
