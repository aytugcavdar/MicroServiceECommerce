using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.Infrastructure.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Domain.Entities.Payment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Method)
            .IsRequired();

        builder.Property(x => x.TransactionId)
            .HasMaxLength(100);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(x => x.OrderId)
            .IsUnique();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.TransactionId);
    }
}
