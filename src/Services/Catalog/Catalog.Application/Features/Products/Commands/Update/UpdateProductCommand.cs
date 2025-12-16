using MediatR;

namespace Catalog.Application.Features.Products.Commands.Update;

public class UpdateProductCommand : IRequest<UpdateProductCommandResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? PictureFileName { get; set; }
    public Guid CategoryId { get; set; }

    public UpdateProductCommand()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
