using System;
using System.Collections.Generic;

namespace Cargo.Domain.Entities
{
    public partial class DebitHistory
    {
        public string DebitHistoryId { get; set; } = null!;
        public string? UserId { get; set; }
        public string? DebitId { get; set; }
        public string? CargoId { get; set; }
        public string? CargoItemId { get; set; }
        public string? CommandName { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual Cargo? Cargo { get; set; }
        public virtual CargoItem? CargoItem { get; set; }
        public virtual Debit? Debit { get; set; }
    }
}
