namespace Catalog.Application.Features.Categories.Commands.Uptade;

public class UpdateCategoryCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime UpdatedDate { get; set; }

    public UpdateCategoryCommandResponse(Guid id, string name, DateTime updatedDate)
    {
        Id = id;
        Name = name;
        UpdatedDate = updatedDate;
    }
}