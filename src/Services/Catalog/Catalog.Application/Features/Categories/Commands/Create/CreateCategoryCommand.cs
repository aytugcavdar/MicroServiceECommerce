using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommand:IRequest<CreateCategoryCommandResponse>
{
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public CreateCategoryCommand()
    {
        Name = string.Empty;
    }
    public CreateCategoryCommand(string name, Guid? parentCategoryId = null)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
    }
}
