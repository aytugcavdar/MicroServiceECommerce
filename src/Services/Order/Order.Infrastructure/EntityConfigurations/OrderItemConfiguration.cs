using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.EntityConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems").HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id).HasColumnName("Id");
        builder.Property(oi => oi.ProductId).HasColumnName("ProductId").IsRequired();
        builder.Property(oi => oi.ProductName).HasColumnName("ProductName").IsRequired().HasMaxLength(200);
        builder.Property(oi => oi.Price).HasColumnName("Price").HasColumnType("decimal(18,2)");
        builder.Property(oi => oi.Quantity).HasColumnName("Quantity");
        builder.Property(oi => oi.OrderId).HasColumnName("OrderId");
    }
}
