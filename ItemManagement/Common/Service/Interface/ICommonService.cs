using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Common.Service.Interface;

public interface ICommonService
{
	Task<CommonUserModel> GetUser();
	List<PurchaseRequestItemResponseModel> CleanAndMergeItemModels(List<PurchaseRequestItemResponseModel> items);
}
