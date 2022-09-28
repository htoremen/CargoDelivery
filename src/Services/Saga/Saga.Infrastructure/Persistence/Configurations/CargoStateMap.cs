using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saga.Domain.Instances;

namespace Saga.Infrastructure.Persistence.Configurations;

public class CargoStateMap : SagaClassMap<CargoStateInstance>
{
    protected override void Configure(EntityTypeBuilder<CargoStateInstance> entity, ModelBuilder model)
    {
        entity.Property(p => p.CurrentState);
        entity.Property(p => p.CourierId).IsRequired();
    }
}
