namespace Route.Domain.Entities
{
    public partial class Route
    {
        public Route()
        {
            CargoEndRoutes = new HashSet<Cargo>();
            CargoStartRoutes = new HashSet<Cargo>();
        }

        public string RouteId { get; set; } = null!;
        public string? Route1 { get; set; }

        public virtual ICollection<Cargo> CargoEndRoutes { get; set; }
        public virtual ICollection<Cargo> CargoStartRoutes { get; set; }
    }
}
