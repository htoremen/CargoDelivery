using System;
using System.Collections.Generic;

namespace Cargo.Domain.Entities
{
    public partial class CargoItem
    {
        public string CargoItemId { get; set; }
        public string CargoId { get; set; }
        public string Barcode { get; set; }
        public string WaybillNumber { get; set; }
        public string Kg { get; set; }
        public string Desi { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        public virtual Cargo Cargo { get; set; }
    }
}
