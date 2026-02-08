using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using System.Reflection;

namespace Payment.Infrastructure.Contexts;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
