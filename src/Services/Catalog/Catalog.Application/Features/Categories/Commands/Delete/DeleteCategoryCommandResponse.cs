using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandResponse
{
    public Guid Id { get; set; }
    public DeleteCategoryCommandResponse(Guid id)
    {
        Id = id;
    }
}
