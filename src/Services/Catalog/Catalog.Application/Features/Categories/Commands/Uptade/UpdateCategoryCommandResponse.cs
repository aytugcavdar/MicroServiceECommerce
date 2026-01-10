namespace Catalog.Application.Features.Categories.Commands.Uptade;

public class UpdateCategoryCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentCategoryId { get; set; } 
    public DateTime UpdatedDate { get; set; }

    public UpdateCategoryCommandResponse() 
    {
    }

    public UpdateCategoryCommandResponse(Guid id, string name, Guid? parentCategoryId, DateTime updatedDate)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategoryId;
        UpdatedDate = updatedDate;
    }
}