using Route.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Route.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CargoRoute> CargoRoutes { get; }
    DbSet<Domain.Entities.Cargo> Cargos { get; }
    DbSet<Domain.Entities.Route> Routes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
