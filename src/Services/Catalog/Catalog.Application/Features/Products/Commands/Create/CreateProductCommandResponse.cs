using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Commands.Create;

public class CreateProductCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedDate { get; set; }

    public CreateProductCommandResponse()
    {
        Name = string.Empty;
    }

    public CreateProductCommandResponse(Guid id, string name, decimal price, int stock, DateTime createdDate)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        CreatedDate = createdDate;
    }
}
