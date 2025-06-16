namespace ItemManagement.Domain.Models.RequestModels;

public class AddItemTypeRequestModel
{
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
}
