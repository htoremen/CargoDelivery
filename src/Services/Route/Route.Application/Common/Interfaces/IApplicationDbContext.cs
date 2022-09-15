using Route.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Route.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CargoRoute> CargoRoutes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
