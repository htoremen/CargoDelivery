using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Saga.Infrastructure.Persistence.Configurations;

namespace Saga.Infrastructure.Persistence;
public class RouteStateDbContext : SagaDbContext
{
    public RouteStateDbContext(DbContextOptions<RouteStateDbContext> options) : base(options) { }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new RouteStateMap();
        }
    }
}

