using System;
using System.Collections.Generic;

namespace Shipment.Domain.Entities
{
    public partial class Cargo
    {
        public string CargoId { get; set; } = null!;
        public string? DebitId { get; set; }
        public int ShipmentTypeId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ShipmentType? ShipmentType { get; set; }
    }
}
