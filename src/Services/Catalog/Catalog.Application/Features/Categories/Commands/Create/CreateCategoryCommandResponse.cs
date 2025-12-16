using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public CreateCategoryCommandResponse()
    {
        Name = string.Empty;
    }
    public CreateCategoryCommandResponse(Guid id, string name, DateTime createdDate)
    {
        Id = id;
        Name = name;
        CreatedDate = createdDate;
    }
}
