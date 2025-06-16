using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Domain.Models.RequestModels;

public class UpdateUserItemWithQuantityRequestModel
{
	public int StatusId { get; set; }
	public string? Comment { get; set; }
	public List<PurchaseRequestItemResponseModel> ItemModels { get; set; } = new();
}
