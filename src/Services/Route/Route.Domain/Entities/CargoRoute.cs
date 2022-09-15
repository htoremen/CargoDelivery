using System;
using System.Collections.Generic;

namespace Route.Domain.Entities;

public partial class CargoRoute
{
    public string CargoRouteId { get; set; } = null!;
    public string? CorrelationId { get; set; }
    public string? CargoId { get; set; }
    public string? Address { get; set; }
    public string? Route { get; set; }
    public DateTime? CreatedOn { get; set; }
    public bool? IsRoute { get; set; }
}