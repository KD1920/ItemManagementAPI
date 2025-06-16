using AutoMapper;
using ItemManagement.Data;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Services;

public class UserService(
	IUserRepository repository,
	IGenericRepository<UserItem> genericRepository,
	IGenericRepository<User> userGenericRepository,
	IGenericRepository<Role> roleGenericRepository,
	IMapper _mapper
) : IUserService
{
	public async Task<RecordTotalRecordResponseHelper<GetUserResponseModel>> GetAllUsersAsync(UserSearchParams searchParams)
	{
		var isExists = await roleGenericRepository.ExistsAsync(x => x.Id == searchParams.RoleId);
		if (!isExists && searchParams.RoleId != 0)
			throw new ArgumentException("Role Id does not exists.");

		var data = await repository.GetAllUsersAsync(searchParams);
		var userAutoMapper = new List<GetUserResponseModel>();
		foreach (var user in data.Records)
		{
			userAutoMapper.Add(_mapper.Map<GetUserResponseModel>(user));
		}
		var response = new RecordTotalRecordResponseHelper<GetUserResponseModel>();
        response.Records = userAutoMapper;
        response.TotalRecords = data.TotalRecords;
		return response;
	}

	public async Task<GetUserWithPasswordResponseModel> GetUserByIdAsync(int id)
	{
		var data = await repository.GetUserByIdAsync(id);
		if (data is null)
			throw new KeyNotFoundException();
		var response = _mapper.Map<GetUserWithPasswordResponseModel>(data);
		return response;
	}
	public async Task<GetUserResponseModel> AddUserAsync(AddUserRequestModel user)
	{
		var isUserNameExists = await userGenericRepository.ExistsAsync(x => x.Name == user.Name);
		if (isUserNameExists)
			throw new ArgumentException("User already exists with the same Name.");
		var isUserEmailExists = await userGenericRepository.ExistsAsync(x => x.Email == user.Email);
		if (isUserEmailExists)
			throw new ArgumentException("User already exists with the same Email.");
		var userAutoMapper = _mapper.Map<User>(user);
		var data = await repository.AddUserAsync(userAutoMapper);
		var response = _mapper.Map<GetUserResponseModel>(data);
		return response;
	}

	public async Task<GetUserResponseModel> UpdateUserAsync(int id, AddUserRequestModel user)
	{
		var userData = await repository.GetUserByIdAsync(id);
		if (userData is null)
			throw new KeyNotFoundException();
		var isUserNameExists = await userGenericRepository.ExistsAsync(x => (x.Name.ToLower() == user.Name.ToLower()) && (userData.Name.ToLower() != user.Name.ToLower()) && (x.Id != id));
		if (isUserNameExists)
			throw new ArgumentException("User already exists with the same Name.");
		var isUserEmailExists = await userGenericRepository.ExistsAsync(x => (x.Email.ToLower() == user.Email.ToLower()) && (userData.Email.ToLower() != user.Email.ToLower()) && (x.Id != id));
		if (isUserEmailExists)
			throw new ArgumentException("User already exists with the same Email.");
		var data = new User();
		var response = new GetUserResponseModel();
		if (userData != null)
		{
			_mapper.Map(user, userData);
			data = await repository.UpdateUserAsync(userData);
			response = _mapper.Map<GetUserResponseModel>(data);
		}
		return response;
	}

	public async Task<GetUserResponseModel> DeleteUserAsync(int id)
	{
		var isExists = await genericRepository.ExistsAsync(x => x.UserId == id && x.Quantity > 0);

		if (isExists)
			throw new ArgumentException("User has some Item assigned to him. Please return them before deleting the user.");

		var userData = await repository.DeleteUserAsync(id);
		if (userData is null)
			throw new KeyNotFoundException();
		var response = _mapper.Map<GetUserResponseModel>(userData);
		return response;
	}
}

