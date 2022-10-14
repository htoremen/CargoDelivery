using Shipment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Shipment.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Cargo> Cargos { get; }
    DbSet<ShipmentType> ShipmentTypes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
