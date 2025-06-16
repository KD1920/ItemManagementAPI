using ItemManagement.Common.Helpers;
using ItemManagement.Data;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Domain.Interface;

public interface IUserRepository
{
	Task<RecordTotalRecordResponseHelper<User>> GetAllUsersAsync(UserSearchParams searchParams);
	Task<User> GetUserByIdAsync(int id);
	Task<User> AddUserAsync(User user);
	Task<User> UpdateUserAsync(User user);
	Task<User> DeleteUserAsync(int id);
}
