namespace ItemManagement.Domain.Models.ResponseModels;

public class PurchaseRequestListResponseModel
{
	public int Id { get; set; }
    public int UserId { get; set; }
	public DateTime? RequestDate { get; set; }
	public string InvoiceNumber { get; set; } = null!;
	public DateTime? CreatedOn { get; set; }
}
