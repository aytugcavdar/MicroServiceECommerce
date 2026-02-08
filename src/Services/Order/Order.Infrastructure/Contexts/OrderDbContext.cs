using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Order.Infrastructure.Contexts;

public class OrderDbContext:DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<OrderSagaState> OrderSagaStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OrderItemSnapshot is only used as JSON column in OrderSagaState, not as a separate entity
        modelBuilder.Ignore<OrderItemSnapshot>();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}
