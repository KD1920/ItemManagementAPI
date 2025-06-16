namespace ItemManagement.Domain.Models.ResponseModels;

public class UserItemRequestResponseModel
{
    public int Id { get; set; }
    public string RequestNumber { get; set; } = null!;
    public int UserId { get; set; }
    public int StatusId { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? Comment { get; set; }
	public List<PurchaseRequestItemResponseModel> ItemModels { get; set; } = new();
    public DateTime? CreatedOn { get; set; }

}
