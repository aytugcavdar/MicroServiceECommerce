using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Commands.Uptade;

public class UpdateCategoryCommand : IRequest<UpdateCategoryCommandResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; } 

    public UpdateCategoryCommand()
    {
    }

    public UpdateCategoryCommand(Guid id, string name, Guid? parentCategoryId)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategoryId;
    }
}