﻿using System;
using System.Collections.Generic;

namespace Shipment.Domain.Entities
{
    public partial class ShipmentType
    {
        public ShipmentType()
        {
            Cargos = new HashSet<Cargo>();
        }

        public string ShipmentTypeId { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? RowNumber { get; set; }

        public virtual ICollection<Cargo> Cargos { get; set; }
    }
}
