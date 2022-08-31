using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saga.Domain.Instances;

namespace Saga.Infrastructure.Persistence.Configurations;
public class RouteStateMap : SagaClassMap<RouteStateInstance>
{
    protected override void Configure(EntityTypeBuilder<RouteStateInstance> entity, ModelBuilder model)
    {
        entity.Property(p => p.UserId).IsRequired();
        entity.Property(p => p.CargoId).IsRequired();
    }
}

