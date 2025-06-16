namespace ItemManagement.Domain.Models.ResponseModels;

public class ItemTypeResponseModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
