using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities;

public class Category : Entity<Guid>, IAggregateRoot
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }
    public ICollection<Product> Products { get; set; }


    public Category()
    {
        Name = string.Empty;
        Products = new HashSet<Product>();
        SubCategories = new HashSet<Category>();
        IsActive = true;
    }
    public Category(string name,Guid? parentCategoryId = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        ParentCategoryId = parentCategoryId;
        IsActive = true;
        Products = new HashSet<Product>();
        SubCategories = new HashSet<Category>();
        CreatedDate = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedDate = DateTime.UtcNow;
    }

    public bool IsRootCategory() => ParentCategoryId == null;

    public bool HasSubCategories() => SubCategories?.Any() == true;
}
