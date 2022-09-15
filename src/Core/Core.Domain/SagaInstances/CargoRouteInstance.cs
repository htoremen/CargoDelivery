using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Instances;
public class CargoRouteInstance
{
    [Key]
    public Guid CargoId { get; set; }
    public string Address { get; set; }
    public string Route { get; set; }
}
