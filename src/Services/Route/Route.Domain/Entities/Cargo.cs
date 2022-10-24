namespace Route.Domain.Entities
{
    public partial class Cargo
    {
        public string CargoId { get; set; } = null!;
        public string? CorrelationId { get; set; }
        public string? StartRouteId { get; set; }
        public string? EndRouteId { get; set; }
        public int? RouteSequence { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual Route? EndRoute { get; set; }
        public virtual Route? StartRoute { get; set; }
    }
}
