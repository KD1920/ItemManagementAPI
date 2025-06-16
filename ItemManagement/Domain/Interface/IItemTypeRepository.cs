using ItemManagement.Data;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Domain.Interface;

public interface IItemTypeRepository
{
	Task<RecordTotalRecordResponseHelper<ItemType>> GetAllItemTypesAsync(CommonRequestHelper commonRequestHelper);
	Task<ItemType> GetItemTypeByIdAsync(int id);
	Task<ItemType> AddItemTypeAsync(ItemType itemType);
	Task<ItemType> UpdateItemTypeAsync(ItemType itemType);
	Task<ItemType> DeleteItemTypeAsync(int id);
}
