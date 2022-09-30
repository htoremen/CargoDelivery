using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cargo.Domain.Entities
{
    public partial class DebitHistory
    {
        public string DebitHistoryId { get; set; }
        public string UserId { get; set; }
        public string DebitId { get; set; }
        public string CargoId { get; set; }
        public string CargoItemId { get; set; }
        public string CommandName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
