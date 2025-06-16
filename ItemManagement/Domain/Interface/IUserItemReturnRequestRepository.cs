using ItemManagement.Common.Helpers;
using ItemManagement.Data;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Domain.Interface;

public interface IUserItemReturnRequestRepository
{
	Task<RecordTotalRecordResponseHelper<ItemReturnRequest>> GetAllItemReturnRequestsAsync(UserItemRequestSearchParams searchParams);
	Task<ItemReturnRequest> GetItemReturnRequestByIdAsync(int id);
	Task<ItemReturnRequest> AddItemReturnRequestAsync(ItemReturnRequest itemReturnRequest);
	Task<ItemReturnRequest> UpdateItemReturnRequestAsync(ItemReturnRequest itemReturnRequest);
	Task<List<ItemReturnRequestItemModel>> GetItemReturnRequestItemModelByIdAsync(int id);
	Task<ItemReturnRequestItemModel> AddItemReturnRequestItemModelAsync(ItemReturnRequestItemModel itemReturnRequestItemModel);
	Task DeleteItemReturnRequestItemModelAsync(int id);
}
