using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Infrastructure.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
    {
        builder.ToTable("Orders").HasKey(o => o.Id);

        builder.Property(o => o.Id).HasColumnName("Id").IsRequired();
        builder.Property(o => o.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(o => o.TotalPrice).HasColumnName("TotalPrice").HasColumnType("decimal(18,2)");
        builder.Property(o => o.CreatedDate).HasColumnName("CreatedDate").IsRequired();

        builder.Property(o => o.Status).HasColumnName("Status").HasConversion<int>();

        builder.OwnsOne(o => o.Address, a =>
        {
            a.Property(x => x.Street).HasColumnName("Address_Street").HasMaxLength(100);
            a.Property(x => x.City).HasColumnName("Address_City").HasMaxLength(50);
            a.Property(x => x.State).HasColumnName("Address_State").HasMaxLength(50);
            a.Property(x => x.Country).HasColumnName("Address_Country").HasMaxLength(50);
            a.Property(x => x.ZipCode).HasColumnName("Address_ZipCode").HasMaxLength(20);
        });

        builder.HasMany(o => o.OrderItems)
               .WithOne()
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade); 
    }
}
