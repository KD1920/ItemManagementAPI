namespace ItemManagement.Domain.Models.RequestModels;

public class PurchaseRequestItemModelRequestModel
{
    public int PurchaseRequestId { get; set; }
    public int ItemModelId { get; set; }
    public int? Quantity { get; set; }
}
