using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Services.Interfaces;

public interface IUserService
{
	Task<RecordTotalRecordResponseHelper<GetUserResponseModel>> GetAllUsersAsync(UserSearchParams searchParams);
	Task<GetUserWithPasswordResponseModel> GetUserByIdAsync(int id);
	Task<GetUserResponseModel> AddUserAsync(AddUserRequestModel user);
	Task<GetUserResponseModel> UpdateUserAsync(int id, AddUserRequestModel user);
	Task<GetUserResponseModel> DeleteUserAsync(int id);
}
