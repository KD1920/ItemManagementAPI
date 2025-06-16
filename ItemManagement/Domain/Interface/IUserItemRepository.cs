using ItemManagement.Data;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Domain.Interface;

public interface IUserItemRepository
{
	Task<List<UserItem>> GetAllUserItemsAsync(UserItemSearchParams searchParams);
	Task AddUserItemQuantityAsync(UserItem userItem);
	Task DeductUserItemQuantityAsync(UserItem userItem);
	Task<bool> UserItemQuantityAvailableAsync(UserItem userItem);
}
