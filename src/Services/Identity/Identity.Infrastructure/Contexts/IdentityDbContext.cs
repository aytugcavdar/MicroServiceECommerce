using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Outbox;
using BuildingBlocks.Core.Security.Constants;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Identity.Infrastructure.Contexts;




public class IdentityDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // Outbox Pattern için
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

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
            u.Property(p => p.RegistrationIp).HasColumnName("RegistrationIp").IsRequired();
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
            oc.HasData(
            new OperationClaim
            {
                Id = Guid.Parse("387f61c5-e5ce-4952-9650-379685655635"), 
                Name = GeneralOperationClaims.Admin,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new OperationClaim
            {
                Id = Guid.Parse("96d29946-f94e-46c5-ab78-36109312130e"), 
                Name = GeneralOperationClaims.User,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
        });

        // UserOperationClaim Configuration
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

        // OutboxMessage Configuration
        modelBuilder.Entity<OutboxMessage>(om =>
        {
            om.ToTable("OutboxMessages").HasKey(k => k.Id);
            om.Property(p => p.Type).HasColumnName("Type").IsRequired().HasMaxLength(500);
            om.Property(p => p.Content).HasColumnName("Content").IsRequired();
            om.Property(p => p.OccurredOn).HasColumnName("OccurredOn").IsRequired();
            om.Property(p => p.ProcessedOn).HasColumnName("ProcessedOn");
            om.Property(p => p.Error).HasColumnName("Error").HasMaxLength(1000);
            om.Property(p => p.RetryCount).HasColumnName("RetryCount").IsRequired();

            // Index'ler (Performans için)
            om.HasIndex(p => new { p.ProcessedOn, p.OccurredOn });
            om.HasIndex(p => p.RetryCount);
        });

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// SaveChanges override - Outbox Pattern implementasyonu
    /// Domain Event'leri OutboxMessages tablosuna kaydeder
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // ============================================
        // 1. DOMAIN EVENT'LERİ TOPLA
        // ============================================
        var domainEntities = ChangeTracker
            .Entries<Entity<Guid>>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        // ============================================
        // 2. DOMAIN EVENT'LERİ ENTİTY'DEN TEMİZLE
        // ============================================
        foreach (var entry in domainEntities)
        {
            entry.Entity.ClearDomainEvents();
        }

        // ============================================
        // 3. DOMAIN EVENT'LERİ OUTBOX'A KAYDET
        // ============================================
        foreach (var domainEvent in domainEvents)
        {
            var eventType = domainEvent.GetType();
            var eventJson = JsonSerializer.Serialize(domainEvent, eventType);

            var outboxMessage = new OutboxMessage(
                type: eventType.Name,
                content: eventJson
            );

            await OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }

        // ============================================
        // 4. HER ŞEYİ BİRLİKTE KAYDET (TRANSACTION)
        // ============================================
        // Users + OutboxMessages aynı transaction içinde
        // Ya ikisi de başarılı, ya ikisi de iptal
        return await base.SaveChangesAsync(cancellationToken);
    }
}