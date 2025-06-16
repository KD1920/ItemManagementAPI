using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;

namespace ItemManagement.Services.Interfaces;

public interface IItemTypeService
{
	Task<RecordTotalRecordResponseHelper<ItemTypeResponseModel>> GetAllItemTypesAsync(CommonRequestHelper commonRequestHelper);
	Task<ItemTypeResponseModel> GetItemTypeByIdAsync(int id);
	Task<ItemTypeResponseModel> AddItemTypeAsync(AddItemTypeRequestModel itemType);
	Task<ItemTypeResponseModel> UpdateItemTypeAsync(int id, AddItemTypeRequestModel itemType);
	Task<ItemTypeResponseModel> DeleteItemTypeAsync(int id);
}
