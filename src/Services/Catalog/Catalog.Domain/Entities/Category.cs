using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities;

public class Category : Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    
    private readonly List<Category> _subCategories = new();
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();
    
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category()
    {
        Name = string.Empty;
    }

    public Category(string name, Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
        ParentCategoryId = parentCategoryId;
        IsActive = true;
        CreatedDate = DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty", nameof(name));
        
        Name = name;
        UpdatedDate = DateTime.UtcNow;
    }

    public void SetParentCategory(Guid? parentCategoryId)
    {
        if (parentCategoryId == Id)
            throw new InvalidOperationException("Category cannot be its own parent");
        
        ParentCategoryId = parentCategoryId;
        UpdatedDate = DateTime.UtcNow;
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

    public bool HasSubCategories() => _subCategories.Count > 0;
}
