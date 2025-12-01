using BuildingBlocks.Core.Domain;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Contexts;

public class CatalogDbContext:DbContext
{
    protected IConfiguration Configuration { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public CatalogDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(p =>
        {
            p.ToTable("Products").HasKey(k=>k.Id);
            p.Property(p=>p.Id).HasColumnName("Id");
            p.Property(p=>p.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            p.Property(p=>p.Description).HasColumnName("Description").IsRequired().HasMaxLength(500);
            p.Property(p => p.Price).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired();

            p.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Category>(c =>
            {
                c.ToTable("Categories").HasKey(k => k.Id);
                c.Property(c => c.Name).IsRequired();
            });

            // Seed Data (Opsiyonel: Veritabanı oluşurken içine örnek veri atar)
            var electronicsGuid = Guid.Parse("d3e20300-8c99-4458-96a4-4e28e461f31f");
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = electronicsGuid,
                    Name = "Electronics"
                }
            );
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => p.DeletedDate == null);
        });

    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<Entity<Guid>>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Eventleri publish et (MediatR ile)
        foreach (var domainEvent in domainEvents)
        {
            // await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
