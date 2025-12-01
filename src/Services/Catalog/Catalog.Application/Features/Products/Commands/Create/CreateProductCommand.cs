using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Commands.Create;

public class CreateProductCommand : IRequest<CreateProductCommandResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? PictureFileName { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }

    public CreateProductCommand()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public CreateProductCommand(string name, string description, decimal price, int stock, string? pictureFileName, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        PictureFileName = pictureFileName;
        CategoryId = categoryId;
    }
}
