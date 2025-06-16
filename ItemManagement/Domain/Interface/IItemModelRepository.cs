using ItemManagement.Data;
using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Domain.Interface;

public interface IItemModelRepository
{
	Task<RecordTotalRecordResponseHelper<ItemModel>> GetAllItemModelsAsync(ItemModelSearchParams searchParams);
	Task<ItemModel> GetItemModelByIdAsync(int id);
	Task<ItemModel> AddItemModelAsync(ItemModel itemModel);
	Task<ItemModel> UpdateItemModelAsync(ItemModel itemModel);
	Task<ItemModel> DeleteItemModelAsync(int id);
	Task AddItemModelQuantityAsync(ItemModel itemModel);
	Task DeductItemModelQuantityAsync(ItemModel itemModel);
	Task<bool> ItemModelQuantityAvailableAsync(PurchaseRequestItemResponseModel itemModel);
}
