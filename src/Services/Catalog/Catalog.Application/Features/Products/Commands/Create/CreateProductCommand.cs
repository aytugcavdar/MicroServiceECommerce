using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Commands.Create;

public class CreateProductCommand:IRequest<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public string? PictureFileName { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }
}
