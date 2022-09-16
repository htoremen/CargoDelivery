using Delivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Delivery> Deliveries { get; }
    DbSet<CargoItem> CargoItems { get; }
    DbSet<Cargo> Cargos { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}