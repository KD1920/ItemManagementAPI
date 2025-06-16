using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Services.Interfaces;

public interface IItemModelService
{
	Task<RecordTotalRecordResponseHelper<ItemModelResponseModel>> GetAllItemModelsAsync(ItemModelSearchParams searchParams);
	Task<ItemModelResponseModel> GetItemModelByIdAsync(int id);
	Task<ItemModelResponseModel> AddItemModelAsync(AddItemModelRequestModel itemModel);
	Task<ItemModelResponseModel> UpdateItemModelAsync(int id, AddItemModelRequestModel itemModel);
	Task<ItemModelResponseModel> DeleteItemModelAsync(int id);
}
