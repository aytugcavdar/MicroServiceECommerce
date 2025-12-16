using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Commands.Update;

public class UpdateProductCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime UpdatedDate { get; set; }

    public UpdateProductCommandResponse(Guid id, string name, decimal price, int stock, DateTime updatedDate)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        UpdatedDate = updatedDate;
    }


}
