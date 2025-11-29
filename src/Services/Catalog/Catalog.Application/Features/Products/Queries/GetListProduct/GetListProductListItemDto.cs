using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Queries.GetListProduct;

public class GetListProductListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } 
}
