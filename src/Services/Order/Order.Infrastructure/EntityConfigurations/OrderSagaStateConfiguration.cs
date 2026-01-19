using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;
using System.Text.Json;

namespace Order.Infrastructure.EntityConfigurations;

public class OrderSagaStateConfiguration : SagaClassMap<OrderSagaState>
{
    protected override void Configure(EntityTypeBuilder<OrderSagaState> entity, ModelBuilder model)
    {
        entity.ToTable("OrderSagaStates");

        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.CorrelationId);
        entity.Property(x => x.RowVersion).IsRowVersion();

        // Eksik olanlar
        entity.Property(x => x.CompletedDate);
        entity.Property(x => x.FailureReason).HasMaxLength(500);
        entity.Property(x => x.RetryCount);
        entity.Property(x => x.StockReservationTokenId);

        // Items için JSON column
        entity.Property(x => x.Items)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<OrderItemSnapshot>>(v, (JsonSerializerOptions)null));
    }
}