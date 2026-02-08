using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities;

public class Product : Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string? PictureFileName { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!; 

    private Product() 
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public Product(string name, string description, decimal price,
                   int stock, string? pictureFileName, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));
        
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));
        
        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative", nameof(stock));

        Id = Guid.NewGuid();
        Name = name;
        Description = description ?? string.Empty;
        Price = price;
        Stock = stock;
        PictureFileName = pictureFileName;
        CategoryId = categoryId;
        CreatedDate = DateTime.UtcNow;
    }

    // Domain Methods
    public void Update(string name, string description, decimal price, Guid categoryId, string? pictureFileName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));
        
        Name = name;
        Description = description ?? string.Empty;
        CategoryId = categoryId;
        
        if (pictureFileName != null)
            PictureFileName = pictureFileName;
        
        UpdatePrice(price);
        UpdatedDate = DateTime.UtcNow;
    }

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

    public void UpdatePictureFileName(string? fileName)
    {
        PictureFileName = fileName;
        UpdatedDate = DateTime.UtcNow;
    }
}