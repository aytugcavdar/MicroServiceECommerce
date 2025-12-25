using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Infrastructure.Contexts;

public class InventoryDbContext:DbContext
{
    protected IConfiguration Configuration { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }

    public InventoryDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryItem>(a =>
        {
            a.ToTable("InventoryItems").HasKey(k => k.Id);
            a.Property(p => p.Id).HasColumnName("Id");
            a.Property(p => p.ProductId).HasColumnName("ProductId").IsRequired();
            a.Property(p => p.Stock).HasColumnName("Stock").IsRequired();
        });
    }
}
