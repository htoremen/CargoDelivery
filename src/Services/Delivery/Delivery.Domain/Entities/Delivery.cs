namespace Delivery.Domain.Entities
{
    public partial class Delivery
    {
        public string DeliveryId { get; set; } = null!;
        public string? CorrelationId { get; set; }
        public string? CargoId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCompleted { get; set; }
        public int? DeliveryType { get; set; }
        public int? PaymentType { get; set; }

        public virtual Cargo? Cargo { get; set; }
    }
}
