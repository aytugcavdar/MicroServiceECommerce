using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Categories.Queries.GetList;

public class GetListCategoryListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; } 
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
