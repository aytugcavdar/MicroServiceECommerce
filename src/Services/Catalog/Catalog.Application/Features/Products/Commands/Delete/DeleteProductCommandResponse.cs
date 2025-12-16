namespace Catalog.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandResponse
{
    public Guid Id { get; set; }
    public DeleteProductCommandResponse(Guid id)
    {
        Id = id;
    }
}
