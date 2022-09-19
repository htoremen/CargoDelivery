using System;
using System.Collections.Generic;

namespace Delivery.Domain.Entities
{
    public partial class CargoItem
    {
        public string CargoItemId { get; set; } = null!;
        public string CargoId { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public string? WaybillNumber { get; set; }
        public string? Kg { get; set; }
        public string? Desi { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }

        public virtual Cargo Cargo { get; set; } = null!;
    }
}
