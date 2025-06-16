namespace ItemManagement.Domain.Models.ResponseModels;

public class PurchaseRequestResponseModel
{
	public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime? RequestDate { get; set; }
    public string InvoiceNumber { get; set; } = null!;
	public List<PurchaseRequestItemResponseModel> ItemModels { get; set; } = new();
    public DateTime? CreatedOn { get; set; }
    public string? UserName { get; set; }
}
