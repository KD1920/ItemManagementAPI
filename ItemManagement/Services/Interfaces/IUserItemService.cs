using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Services.Interfaces;

public interface IUserItemService
{
	Task<List<UserItemResponseModel>> GetAllUserItemsAsync(UserItemSearchParams searchParams);
}
