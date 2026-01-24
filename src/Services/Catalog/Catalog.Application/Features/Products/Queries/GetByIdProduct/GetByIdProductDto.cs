
using System;

namespace Catalog.Application.Features.Products.Queries.GetByIdProduct;

public class GetByIdProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; } // Generic name for PictureFileName
    public string CategoryName { get; set; }
}
