using ItemManagement.Data;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Domain.Interface;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Repository;

public class UserRepository(ItemManagementDbContext _context, IGenericRepository<User> _repository) : IUserRepository
{
	public async Task<RecordTotalRecordResponseHelper<User>> GetAllUsersAsync(UserSearchParams searchParams)
	{
		var query = _context.Users.AsQueryable();

		if (!string.IsNullOrWhiteSpace(searchParams.Search))
		{
			var searchLower = searchParams.Search.ToLower();
			query = query.Where(
				x => x.Name.ToLower().Contains(searchLower) ||
				x.Email.ToLower().Contains(searchLower)
			);
		}

		if (searchParams.RoleId != null && searchParams.RoleId != 0)
		{
			query = query.Where(x => x.RoleId == searchParams.RoleId);
		}

		if (searchParams.Active != null)
		{
			query = query.Where(x => x.Active == searchParams.Active);
		}

		query = (searchParams.SortBy?.ToLower(), searchParams.SortOrder?.ToLower()) switch
		{
			("name", "asc") => query.OrderBy(x => x.Name),
			("name", "desc") => query.OrderByDescending(x => x.Name),
			("email", "asc") => query.OrderBy(x => x.Email),
			("email", "desc") => query.OrderByDescending(x => x.Email),
			("roleid", "asc") => query.OrderBy(x => x.RoleId),
			("roleid", "desc") => query.OrderByDescending(x => x.RoleId),
			("active", "asc") => query.OrderBy(x => x.Active),
			("active", "desc") => query.OrderByDescending(x => x.Active),
			_ => query.OrderBy(x => x.Name)
		};

		var pageNumber = (searchParams.Page > 0 ? searchParams.Page : 1) - 1;
		var pageSize = searchParams.PageSize > 0 ? searchParams.PageSize : 10;

		var skip = pageNumber * pageSize;

		var data = await query.Skip(skip).Take(pageSize).ToListAsync();
		var total = await query.CountAsync();

		var response = new RecordTotalRecordResponseHelper<User>();
		response.Records = data;
		response.TotalRecords = total;
		return response;
	}


	public async Task<User> GetUserByIdAsync(int id)
	{
		return await _repository.GetByIdAsync(id);
	}

	public async Task<User> AddUserAsync(User user)
	{
		return await _repository.AddAsync(user);
	}

	public async Task<User> UpdateUserAsync(User user)
	{
		return await _repository.UpdateAsync(user);
	}

	public async Task<User> DeleteUserAsync(int id)
	{
		return await _repository.DeleteAsync(id);
	}
}
