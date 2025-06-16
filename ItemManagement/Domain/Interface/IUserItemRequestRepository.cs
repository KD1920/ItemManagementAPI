using ItemManagement.Common.Helpers;
using ItemManagement.Data;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Domain.Interface;

public interface IUserItemRequestRepository
{
	Task<RecordTotalRecordResponseHelper<ItemRequest>> GetAllItemRequestsAsync(UserItemRequestSearchParams searchParams);
	Task<ItemRequest> GetItemRequestByIdAsync(int id);
	Task<ItemRequest> AddItemRequestAsync(ItemRequest itemRequest);
	Task<ItemRequest> UpdateItemRequestAsync(ItemRequest itemRequest);
	Task<List<ItemRequestItemModel>> GetItemRequestItemModelByIdAsync(int id);
	Task<ItemRequestItemModel> AddItemRequestItemModelAsync(ItemRequestItemModel itemRequestItem);
	Task DeleteItemRequestItemModelAsync(int id);
}

