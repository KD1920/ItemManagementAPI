using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Services.Interfaces;

public interface IUserItemRequestService
{
	Task<RecordTotalRecordResponseHelper<UserItemRequestResponseModel>> GetAllItemRequestsAsync(UserItemRequestSearchParams searchParams);
	Task<UserItemRequestResponseModel> GetItemRequestByIdAsync(int id);
	Task<UserItemRequestResponseModel> AddItemRequestAsync(AddUserItemWithQuantityRequestModel itemRequest, int StatusId, int userId);
	Task<UserItemRequestResponseModel> UpdateItemRequestAsync(int id, UpdateUserItemWithQuantityRequestModel itemRequest);
}
