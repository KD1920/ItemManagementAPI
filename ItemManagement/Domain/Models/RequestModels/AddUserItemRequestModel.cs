namespace ItemManagement.Domain.Models.RequestModels;

public class AddUserItemRequestModel
{
	public string RequestNumber { get; set; } = null!;
	public int UserId { get; set; }
	public int StatusId { get; set; }
	public DateTime? RequestDate { get; set; }
	public string? Comment { get; set; }
}
