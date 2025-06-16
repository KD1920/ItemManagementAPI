using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Services.Interfaces;

public interface IUserItemReturnRequestService
{
	Task<RecordTotalRecordResponseHelper<UserItemRequestResponseModel>> GetAllItemReturnRequestsAsync(UserItemRequestSearchParams searchParams);
	Task<UserItemRequestResponseModel> GetItemReturnRequestByIdAsync(int id);
	Task<UserItemRequestResponseModel> AddItemReturnRequestAsync(AddUserItemWithQuantityRequestModel itemReturnRequest, int StatusId, int userId);
	Task<UserItemRequestResponseModel> UpdateItemReturnRequestAsync(int id, UpdateUserItemWithQuantityRequestModel itemReturnRequest, int userId);
}
