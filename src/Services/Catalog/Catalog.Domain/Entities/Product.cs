using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities;

public class Product : Entity<Guid>, IAggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? PictureFileName { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!; 

    private Product() { }

    public Product(string name, string description, decimal price,
                   int stock, string? pictureFileName, Guid categoryId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        PictureFileName = pictureFileName;
        CategoryId = categoryId;
        CreatedDate = DateTime.UtcNow;
    }

    // Domain Methods
    public void UpdateStock(int newStock)
    {
        if (newStock < 0)
            throw new InvalidOperationException("Stock cannot be negative");

        Stock = newStock;
        UpdatedDate = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new InvalidOperationException("Price must be greater than zero");

        Price = newPrice;
        UpdatedDate = DateTime.UtcNow;
    }
}