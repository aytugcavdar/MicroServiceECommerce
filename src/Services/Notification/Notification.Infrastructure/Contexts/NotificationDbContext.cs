using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Contexts;

public class NotificationDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }

    public DbSet<NotificationLog> NotificationLogs { get; set; }

    public NotificationDbContext(
        DbContextOptions<NotificationDbContext> options,
        IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationLog>(nl =>
        {
            nl.ToTable("NotificationLogs").HasKey(k => k.Id);
            nl.Property(p => p.Type).IsRequired();
            nl.Property(p => p.RecipientEmail).IsRequired().HasMaxLength(100);
            nl.Property(p => p.RecipientPhone).HasMaxLength(20);
            nl.Property(p => p.Subject).IsRequired().HasMaxLength(200);
            nl.Property(p => p.Content).IsRequired();
            nl.Property(p => p.Status).IsRequired();
            nl.Property(p => p.ErrorMessage).HasMaxLength(1000);
            nl.Property(p => p.TemplateName).HasMaxLength(100);
            nl.Property(p => p.EventType).HasMaxLength(100);
            nl.Property(p => p.RetryCount).IsRequired();

            // Indexes
            nl.HasIndex(p => p.RecipientEmail);
            nl.HasIndex(p => p.Status);
            nl.HasIndex(p => p.EventType);
            nl.HasIndex(p => new { p.Status, p.CreatedDate });

            nl.HasQueryFilter(n => n.DeletedDate == null);
        });

        base.OnModelCreating(modelBuilder);
    }
}
