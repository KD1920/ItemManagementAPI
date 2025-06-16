using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Domain.Models.RequestModels;

public class PurchaseRequestWithItemModels
{
	public List<PurchaseRequestItemResponseModel> ItemModels { get; set; } = new();
}
