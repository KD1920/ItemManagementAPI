using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Domain.Models.RequestModels;

public class AddUserItemWithQuantityRequestModel
{
	public List<PurchaseRequestItemResponseModel> ItemModels { get; set; } = new();
}
