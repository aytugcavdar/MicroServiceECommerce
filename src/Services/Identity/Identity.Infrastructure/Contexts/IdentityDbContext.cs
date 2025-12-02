using BuildingBlocks.Core.Domain;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastructure.Contexts;

public class IdentityDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options,
        IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User Configuration
        modelBuilder.Entity<User>(u =>
        {
            u.ToTable("Users").HasKey(k => k.Id);
            u.Property(p => p.Id).HasColumnName("Id");
            u.Property(p => p.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(50);
            u.Property(p => p.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(50);
            u.Property(p => p.Email).HasColumnName("Email").IsRequired().HasMaxLength(100);
            u.Property(p => p.UserName).HasColumnName("UserName").IsRequired().HasMaxLength(50);
            u.Property(p => p.PasswordHash).HasColumnName("PasswordHash").IsRequired();
            u.Property(p => p.PasswordSalt).HasColumnName("PasswordSalt").IsRequired();
            u.Property(p => p.Status).HasColumnName("Status").IsRequired();
            u.Property(p => p.IsEmailConfirmed).HasColumnName("IsEmailConfirmed").IsRequired();
            u.Property(p => p.EmailConfirmationToken).HasColumnName("EmailConfirmationToken").HasMaxLength(255);

            u.HasIndex(p => p.Email).IsUnique();
            u.HasIndex(p => p.UserName).IsUnique();

            // Soft Delete Query Filter
            u.HasQueryFilter(p => p.DeletedDate == null);
        });

        // OperationClaim Configuration
        modelBuilder.Entity<OperationClaim>(oc =>
        {
            oc.ToTable("OperationClaims").HasKey(k => k.Id);
            oc.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            oc.HasIndex(p => p.Name).IsUnique();
        });

        // UserOperationClaim Configuration (Many-to-Many)
        modelBuilder.Entity<UserOperationClaim>(uoc =>
        {
            uoc.ToTable("UserOperationClaims").HasKey(k => k.Id);

            uoc.HasOne(p => p.User)
               .WithMany(u => u.UserOperationClaims)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            uoc.HasOne(p => p.OperationClaim)
               .WithMany(oc => oc.UserOperationClaims)
               .HasForeignKey(p => p.OperationClaimId)
               .OnDelete(DeleteBehavior.Cascade);

            // Bir kullanıcı aynı rolü birden fazla kez alamaz
            uoc.HasIndex(p => new { p.UserId, p.OperationClaimId }).IsUnique();
        });

        // RefreshToken Configuration
        modelBuilder.Entity<RefreshToken>(rt =>
        {
            rt.ToTable("RefreshTokens").HasKey(k => k.Id);
            rt.Property(p => p.Token).HasColumnName("Token").IsRequired().HasMaxLength(500);
            rt.Property(p => p.ExpiresAt).HasColumnName("ExpiresAt").IsRequired();
            rt.Property(p => p.CreatedByIp).HasColumnName("CreatedByIp").IsRequired().HasMaxLength(50);

            rt.HasOne(p => p.User)
              .WithMany(u => u.RefreshTokens)
              .HasForeignKey(p => p.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            rt.HasIndex(p => p.Token);
        });

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Domain Events varsa publish et
        var domainEntities = ChangeTracker.Entries<Entity<Guid>>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // TODO: Domain events publish et (MediatR ile)
        // foreach (var domainEvent in domainEvents)
        // {
        //     await _mediator.Publish(domainEvent, cancellationToken);
        // }

        return result;
    }
}