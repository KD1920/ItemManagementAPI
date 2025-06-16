using ItemManagement.Data;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Domain.Interface;
using ItemManagement.Domain.Models.SearchParamModels;

namespace ItemManagement.Repository;

public class UserItemRepository(ItemManagementDbContext _context) : IUserItemRepository
{
	public async Task<List<UserItem>> GetAllUserItemsAsync(UserItemSearchParams searchParams)
	{
		var query = _context.UserItems.AsQueryable();

		if (searchParams.UserId != null && searchParams.UserId != 0)
		{
			query = query.Where(x => x.UserId == searchParams.UserId);
		}


		if (searchParams.ItemModelId != null && searchParams.ItemModelId != 0)
		{
			query = query.Where(x => x.ItemModelId == searchParams.ItemModelId);
		}

		query = (searchParams.SortBy?.ToLower(), searchParams.SortOrder?.ToLower()) switch
		{
			("itemmodelid", "asc") => query.OrderBy(x => x.ItemModelId),
			("itemmodelid", "desc") => query.OrderByDescending(x => x.ItemModelId),
			("userid", "asc") => query.OrderBy(x => x.UserId),
			("userid", "desc") => query.OrderByDescending(x => x.UserId),
			_ => query.OrderBy(x => x.UserId)
		};

		var pageNumber = (searchParams.Page > 0 ? searchParams.Page : 1) - 1;
		var pageSize = searchParams.PageSize > 0 ? searchParams.PageSize : 10;

		var skip = pageNumber * pageSize;

		var data = await query.Skip(skip).Take(pageSize).ToListAsync();

		return data;
	}

	public async Task AddUserItemQuantityAsync(UserItem userItem)
	{
		var data = await _context.UserItems.FirstOrDefaultAsync(u => u.UserId == userItem.UserId && u.ItemModelId == userItem.ItemModelId);
		if (data != null)
		{
			data.Quantity += userItem.Quantity;
			_context.UserItems.Update(data);
		}
		else
		{
			await _context.UserItems.AddAsync(userItem);
		}
		await _context.SaveChangesAsync();
	}

	public async Task DeductUserItemQuantityAsync(UserItem userItem)
	{
		var data = await _context.UserItems.FirstOrDefaultAsync(u => u.UserId == userItem.UserId && u.ItemModelId == userItem.ItemModelId);
		if (data != null && data.Quantity >= userItem.Quantity)
		{
			data.Quantity -= userItem.Quantity;
			_context.UserItems.Update(data);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<bool> UserItemQuantityAvailableAsync(UserItem userItem)
	{
		var data = await _context.UserItems.FirstOrDefaultAsync(u => u.UserId == userItem.UserId && u.ItemModelId == userItem.ItemModelId);
		var isAvailable = (data != null) && (data.Quantity >= userItem.Quantity);
		return isAvailable;
	}
}
