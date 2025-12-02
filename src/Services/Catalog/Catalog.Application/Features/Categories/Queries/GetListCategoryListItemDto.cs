using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Queries;

public class GetListCategoryListItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
