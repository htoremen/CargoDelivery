using Cargo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cargo.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Cargo> Cargos { get; }
    DbSet<CargoItem> CargoItems { get; }
    DbSet<Debit> Debits { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
