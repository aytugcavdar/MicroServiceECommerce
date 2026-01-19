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
            p.ToTable("Products").HasKey(k => k.Id);
            p.Property(p => p.Id).HasColumnName("Id");
            p.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            p.Property(p => p.Description).HasColumnName("Description").IsRequired().HasMaxLength(500);
            p.Property(p => p.Price).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired();
            p.Property(p => p.Stock).HasColumnName("Stock").IsRequired();
            p.Property(p => p.PictureFileName).HasColumnName("PictureFileName").HasMaxLength(255);

            p.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); 

            p.HasQueryFilter(p => p.DeletedDate == null);
        });

        modelBuilder.Entity<Category>(c =>
        {
            c.ToTable("Categories").HasKey(k => k.Id);
            c.Property(c => c.Id).HasColumnName("Id");
            c.Property(c => c.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            c.Property(c => c.IsActive).HasColumnName("IsActive").IsRequired();
            c.Property(c => c.ParentCategoryId).HasColumnName("ParentCategoryId");

            c.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            c.Navigation(c => c.ParentCategory).AutoInclude(false);
            c.Navigation(c => c.SubCategories).AutoInclude(false);

            c.HasIndex(c => c.ParentCategoryId);
            c.HasIndex(c => c.IsActive);

            c.HasQueryFilter(c => c.DeletedDate == null);
        });

        var electronicsId = Guid.Parse("d3e20300-8c99-4458-96a4-4e28e461f31f");
        var phonesId = Guid.Parse("a1b2c3d4-e5f6-7890-1234-567890abcdef");
        var laptopsId = Guid.Parse("f1e2d3c4-b5a6-9780-4321-fedcba098765");

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = electronicsId,
                Name = "Electronics",
                IsActive = true,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                Id = phonesId,
                Name = "Mobile Phones",
                ParentCategoryId = electronicsId,
                IsActive = true,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                Id = laptopsId,
                Name = "Laptops",
                ParentCategoryId = electronicsId,
                IsActive = true,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        base.OnModelCreating(modelBuilder);

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

        foreach (var domainEvent in domainEvents)
        {
            // await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
