namespace Route.Domain.Entities
{
    public partial class Route
    {
        public string RouteId { get; set; } = null!;
        public string? RouteAddress { get; set; }
        public string? CorrelationId { get; set; }
        public bool IsCurrentRoute { get; set; }
    }
}
