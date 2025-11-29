using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities;

public class Product:Entity<Guid>, IAggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? PictureFileName { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public Product(string name, string description, decimal price, int stock, string? pictureFileName, Guid categoryId, Category category)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        PictureFileName = pictureFileName;
        CategoryId = categoryId;
        Category = category;
    }

    public Product()
    {
        
    }

    public Product(string name, string description, decimal price, int stock, string? pictureFileName, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        PictureFileName = pictureFileName;
        CategoryId = categoryId;
    }
}
