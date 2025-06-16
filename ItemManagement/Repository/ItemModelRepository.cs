using ItemManagement.Data;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Domain.Interface;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Repository;

public class ItemModelRepository(ItemManagementDbContext _context, IGenericRepository<ItemModel> _repository) : IItemModelRepository
{
	public async Task<RecordTotalRecordResponseHelper<ItemModel>> GetAllItemModelsAsync(ItemModelSearchParams searchParams)
	{
		var query = _context.ItemModels.AsQueryable();

		if (!string.IsNullOrWhiteSpace(searchParams.Search))
		{
			var searchLower = searchParams.Search.ToLower();
			query = query.Where(
				x => x.Name.ToLower().Contains(searchLower) ||
				x.Description.ToLower().Contains(searchLower)
			);
		}
		if (searchParams.ItemTypeId > 0)
		{
			query = query.Where(x => x.ItemTypeId == searchParams.ItemTypeId);
		}

		query = (searchParams.SortBy?.ToLower(), searchParams.SortOrder?.ToLower()) switch
		{
			("name", "asc") => query.OrderBy(x => x.Name),
			("name", "desc") => query.OrderByDescending(x => x.Name),
			("itemtypeid", "asc") => query.OrderBy(x => x.ItemTypeId).ThenBy(x => x.Name),
			("itemtypeid", "desc") => query.OrderByDescending(x => x.ItemTypeId).ThenBy(x => x.Name),
			("quantity", "asc") => query.OrderBy(x => x.Quantity).ThenBy(x => x.Name),
			("quantity", "desc") => query.OrderByDescending(x => x.Quantity).ThenBy(x => x.Name),
			_ => query.OrderBy(x => x.Name)
		};

		var pageNumber = (searchParams.Page > 0 ? searchParams.Page : 1) - 1;
		var pageSize = searchParams.PageSize > 0 ? searchParams.PageSize : 10;

		var skip = pageNumber * pageSize;

		var data = await query.Skip(skip).Take(pageSize).ToListAsync();
		var total = await query.CountAsync();

		var response = new RecordTotalRecordResponseHelper<ItemModel>();
		response.Records = data;
		response.TotalRecords = total;
		return response;
	}

	public async Task<ItemModel> GetItemModelByIdAsync(int id)
	{
		return await _repository.GetByIdAsync(id);
	}

	public async Task<ItemModel> AddItemModelAsync(ItemModel itemModel)
	{
		return await _repository.AddAsync(itemModel);
	}

	public async Task<ItemModel> UpdateItemModelAsync(ItemModel itemModel)
	{
		return await _repository.UpdateAsync(itemModel);
	}

	public async Task<ItemModel> DeleteItemModelAsync(int id)
	{
		return await _repository.DeleteAsync(id);
	}

	public async Task AddItemModelQuantityAsync(ItemModel itemModel)
	{
		var data = await _context.ItemModels.FirstOrDefaultAsync(u => u.Id == itemModel.Id);
		data.Quantity += itemModel.Quantity;
		_context.ItemModels.Update(data);
		await _context.SaveChangesAsync();
	}

	public async Task DeductItemModelQuantityAsync(ItemModel itemModel)
	{
		var data = await _context.ItemModels.FirstOrDefaultAsync(u => u.Id == itemModel.Id);
		if (data.Quantity >= itemModel.Quantity)
		{
			data.Quantity -= itemModel.Quantity;
			_context.ItemModels.Update(data);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<bool> ItemModelQuantityAvailableAsync(PurchaseRequestItemResponseModel itemModel)
	{
		var data = await _context.ItemModels.FirstOrDefaultAsync(u => u.Id == itemModel.ItemModelId);
		var isAvailable = (data != null) && (data.Quantity >= itemModel.Quantity);
		return isAvailable;
	}
}
