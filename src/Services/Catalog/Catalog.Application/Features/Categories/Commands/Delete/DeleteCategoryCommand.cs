using MediatR;

namespace Catalog.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommand : IRequest<DeleteCategoryCommandResponse>
{
    public Guid Id { get; set; }

    public DeleteCategoryCommand(Guid id)
    {
        Id = id;
    }
}
