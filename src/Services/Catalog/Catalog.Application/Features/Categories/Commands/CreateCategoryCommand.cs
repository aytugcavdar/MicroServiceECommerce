using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Commands;

public class CreateCategoryCommand:IRequest<CreateCategoryCommandResponse>
{
    public string Name { get; set; }
    public CreateCategoryCommand()
    {
        Name = string.Empty;
    }
    public CreateCategoryCommand(string name)
    {
        Name = name;
    }
}
