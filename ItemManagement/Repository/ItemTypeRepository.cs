using ItemManagement.Data;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Common.Helpers;
using ItemManagement.Domain.Interface;

namespace ItemManagement.Repository;

public class ItemTypeRepository(ItemManagementDbContext _context, IGenericRepository<ItemType> _repository) : IItemTypeRepository
{
	public async Task<RecordTotalRecordResponseHelper<ItemType>> GetAllItemTypesAsync(CommonRequestHelper searchParams)
	{
		var query = _context.ItemTypes.AsQueryable();

		if (!string.IsNullOrWhiteSpace(searchParams.Search))
		{
			var searchLower = searchParams.Search.ToLower();
			query = query.Where(
				x => x.Name.ToLower().Contains(searchLower) ||
				x.Description.ToLower().Contains(searchLower)
			);
		}

		query = (searchParams.SortBy?.ToLower(), searchParams.SortOrder?.ToLower()) switch
		{
			("name", "asc") => query.OrderBy(x => x.Name),
			("name", "desc") => query.OrderByDescending(x => x.Name),
			_ => query.OrderBy(x => x.Name)
		};

		var pageNumber = (searchParams.Page > 0 ? searchParams.Page : 1) - 1;
		var pageSize = searchParams.PageSize > 0 ? searchParams.PageSize : 10;

		var skip = pageNumber * pageSize;

		var data = await query.Skip(skip).Take(pageSize).ToListAsync();
		var total = await query.CountAsync();

		var response = new RecordTotalRecordResponseHelper<ItemType>();
		response.Records = data;
		response.TotalRecords = total;
		return response;
	}

	public async Task<ItemType> GetItemTypeByIdAsync(int id)
	{
		return await _repository.GetByIdAsync(id);
	}

	public async Task<ItemType> AddItemTypeAsync(ItemType itemType)
	{
		return await _repository.AddAsync(itemType);
	}

	public async Task<ItemType> UpdateItemTypeAsync(ItemType itemType)
	{
		return await _repository.UpdateAsync(itemType);
	}

	public async Task<ItemType> DeleteItemTypeAsync(int id)
	{
		return await _repository.DeleteAsync(id);
	}
}
