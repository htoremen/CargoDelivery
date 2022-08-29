using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Saga.Infrastructure.Persistence.Configurations;

namespace Saga.Infrastructure.Persistence;

public class CargoStateDbContext : SagaDbContext
{
    public CargoStateDbContext(DbContextOptions<CargoStateDbContext> options) : base(options) { }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new CargoStateMap();
        }
    }
}

