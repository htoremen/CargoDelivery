namespace Route.Domain.Entities
{
    public partial class Route
    {
        public string RouteId { get; set; } = null!;
        public string? CorrelationId { get; set; }
        public string? RouteAddress { get; set; }
        public bool? IsCurrentRoute { get; set; }
    }
}
