using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.EntityConfigurations;

public class OrderSagaStateConfiguration : SagaClassMap<OrderSagaState>
{
    protected override void Configure(EntityTypeBuilder<OrderSagaState> entity, ModelBuilder model)
    {
        entity.ToTable("OrderSagaStates");

        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.CorrelationId); 

        entity.Property(x => x.RowVersion).IsRowVersion();
    }
}