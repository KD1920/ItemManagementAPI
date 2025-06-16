using AutoMapper;
using ItemManagement.Data;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.Common.Service.Interface;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Services;

public class UserItemService(
	IUserItemRepository repository,
	IGenericRepository<User> userGenericRepository,
	IGenericRepository<ItemModel> itemModelGenericRepository,
	IMapper _mapper,
	ICommonService commonService
) : IUserItemService
{
	
	public async Task<List<UserItemResponseModel>> GetAllUserItemsAsync(UserItemSearchParams searchParams)
	{
		var userData = await userGenericRepository.ExistsAsync(x => x.Id == searchParams.UserId);
		if (userData == null)
			throw new ArgumentException("User does not exists");
		var itemModelData = await itemModelGenericRepository.ExistsAsync(x => x.Id == searchParams.ItemModelId);
		if (itemModelData == null)
			throw new ArgumentException("Item does not exists");
		var data = await repository.GetAllUserItemsAsync(searchParams);
		if (data is null)
			throw new KeyNotFoundException();
		var currentUser = await commonService.GetUser();
		var userItemAutoMapper = new List<UserItemResponseModel>();
		foreach (var user in data)
		{
			if (user.UserId == currentUser.UserId || currentUser.IsAdmin)
				userItemAutoMapper.Add(_mapper.Map<UserItemResponseModel>(user));
		}
		return userItemAutoMapper;
	}
}
