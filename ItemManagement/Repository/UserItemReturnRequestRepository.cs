using ItemManagement.Data;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Domain.Interface;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Repository;

public class UserItemReturnRequestRepository(ItemManagementDbContext _context, IGenericRepository<ItemReturnRequest> _repository, IGenericRepository<ItemReturnRequestItemModel> _itemModelRepository) : IUserItemReturnRequestRepository
{
	public async Task<RecordTotalRecordResponseHelper<ItemReturnRequest>> GetAllItemReturnRequestsAsync(UserItemRequestSearchParams searchParams)

	{
		var query = _context.ItemReturnRequests.AsQueryable();

		if (!string.IsNullOrWhiteSpace(searchParams.Search))
		{
			var searchLower = searchParams.Search.ToLower();
			query = query.Where(x => x.RequestNumber.ToLower().Contains(searchLower));
		}

		if (searchParams.RequestDate != null)
		{
			query = query.Where(x => x.RequestDate == searchParams.RequestDate);
		}

		if (searchParams.RequestNumber != null)
		{
			query = query.Where(x => x.RequestNumber == searchParams.RequestNumber);
		}

		if (searchParams.UserId != null && searchParams.UserId != 0)
		{
			query = query.Where(x => x.UserId == searchParams.UserId);
		}

		if (searchParams.StatusId != null && searchParams.StatusId != 0)
		{
			query = query.Where(x => x.StatusId == searchParams.StatusId);
		}

		query = (searchParams.SortBy?.ToLower(), searchParams.SortOrder?.ToLower()) switch
		{
			("requestnumber", "asc") => query.OrderBy(x => x.RequestNumber),
			("requestnumber", "desc") => query.OrderByDescending(x => x.RequestNumber),
			("statusid", "asc") => query.OrderBy(x => x.StatusId),
			("statusid", "desc") => query.OrderByDescending(x => x.StatusId),
			("requestdate", "asc") => query.OrderBy(x => x.RequestDate),
			("requestdate", "desc") => query.OrderByDescending(x => x.RequestDate),
			("userid", "asc") => query.OrderBy(x => x.UserId),
			("userid", "desc") => query.OrderByDescending(x => x.UserId),
			_ => query.OrderByDescending(x => x.RequestNumber)
		};

		var pageNumber = (searchParams.Page > 0 ? searchParams.Page : 1) - 1;
		var pageSize = searchParams.PageSize > 0 ? searchParams.PageSize : 10;

		var skip = pageNumber * pageSize;

		var data = await query.Skip(skip).Take(pageSize).ToListAsync();
		var total = await query.CountAsync();

		var response = new RecordTotalRecordResponseHelper<ItemReturnRequest>();
		response.Records = data;
		response.TotalRecords = total;
		return response;
	}

	public async Task<ItemReturnRequest> GetItemReturnRequestByIdAsync(int id)
	{
		return await _repository.GetByIdAsync(id);
	}

	public async Task<ItemReturnRequest> AddItemReturnRequestAsync(ItemReturnRequest itemRequest)
	{
		return await _repository.AddAsync(itemRequest);
	}

	public async Task<ItemReturnRequest> UpdateItemReturnRequestAsync(ItemReturnRequest itemRequest)
	{
		return await _repository.UpdateAsync(itemRequest);
	}

	public async Task<List<ItemReturnRequestItemModel>> GetItemReturnRequestItemModelByIdAsync(int id)
	{
		return await _itemModelRepository.GetByConditionAsync(value => value.ItemRequestId == id);
	}

	public async Task<ItemReturnRequestItemModel> AddItemReturnRequestItemModelAsync(ItemReturnRequestItemModel itemRequestItemModel)
	{
		return await _itemModelRepository.AddAsync(itemRequestItemModel);
	}

	public async Task DeleteItemReturnRequestItemModelAsync(int id)
	{
		var data = await _itemModelRepository.GetByConditionAsync(x => x.ItemRequestId == id);
		_itemModelRepository.DeleteRangeAsync(data);
	}
}
