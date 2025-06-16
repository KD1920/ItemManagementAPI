namespace ItemManagement.Domain.Models.RequestModels;

public class AddItemModelRequestModel
{
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public int ItemTypeId { get; set; }
}
