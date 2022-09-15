using Microsoft.EntityFrameworkCore;
using Route.Domain.Entities;

namespace Route.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CargoRoute> CargoRoutes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
