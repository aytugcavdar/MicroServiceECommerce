using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities;

public class Category:Entity<Guid>, IAggregateRoot
{
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }

    public Category()
    {
        Products = new HashSet<Product>();
    }
    public Category(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
