using ItemManagement.Data;
using Microsoft.EntityFrameworkCore;
using ItemManagement.Domain.Interface;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Service.Interface;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Repository;

public class PurchaseRequestRepository(
	ItemManagementDbContext _context,
	IGenericRepository<PurchaseRequest> _repository,
	IGenericRepository<PurchaseRequestItemModel> _itemModelRepository,
	ICommonService commonService
) : IPurchaseRequestRepository
{
	public async Task<RecordTotalRecordResponseHelper<PurchaseRequest>> GetAllPurchaseRequestsAsync(PurchaseRequestSearchParams searchParams)
	{
		var query = _context.PurchaseRequests.AsQueryable();

		var user = await commonService.GetUser();

		query = query.Where(x => x.CreatedBy == user.UserId);

		if (!string.IsNullOrWhiteSpace(searchParams.Search))
		{
			var searchLower = searchParams.Search.ToLower();
			query = query.Where(x => x.InvoiceNumber.ToLower().Contains(searchLower));
		}

		if (searchParams.RequestDate != null)
		{
			query = query.Where(x => x.RequestDate == searchParams.RequestDate);
		}

		if (searchParams.UserId != null && searchParams.UserId != 0)
		{
			query = query.Where(x => x.UserId == searchParams.UserId);
		}

		query = (searchParams.SortBy?.ToLower(), searchParams.SortOrder?.ToLower()) switch
		{
			("invoicenumber", "asc") => query.OrderBy(x => x.InvoiceNumber),
			("invoicenumber", "desc") => query.OrderByDescending(x => x.InvoiceNumber),
			("requestdate", "asc") => query.OrderBy(x => x.RequestDate),
			("requestdate", "desc") => query.OrderByDescending(x => x.RequestDate),
			("userid", "asc") => query.OrderBy(x => x.UserId),
			("userid", "desc") => query.OrderByDescending(x => x.UserId),
			_ => query.OrderByDescending(x => x.InvoiceNumber)
		};

		var pageNumber = (searchParams.Page > 0 ? searchParams.Page : 1) - 1;
		var pageSize = searchParams.PageSize > 0 ? searchParams.PageSize : 10;

		var skip = pageNumber * pageSize;

		var data = await query.Skip(skip).Take(pageSize).ToListAsync();
		var total = await query.CountAsync();

		var response = new RecordTotalRecordResponseHelper<PurchaseRequest>();
		response.Records = data;
		response.TotalRecords = total;
		return response;
	}

	public async Task<PurchaseRequest> GetPurchaseRequestByIdAsync(int id)
	{
		return await _repository.GetByIdAsync(id);
	}

	public async Task<PurchaseRequest> AddPurchaseRequestAsync(PurchaseRequest purchaseRequest)
	{
		return await _repository.AddAsync(purchaseRequest);
	}

	public async Task<List<PurchaseRequestItemModel>> GetPurchaseRequestItemModelByIdAsync(int id)
	{
		return await _itemModelRepository.GetByConditionAsync(value => value.PurchaseRequestId == id);
	}

	public async Task<PurchaseRequestItemModel> AddPurchaseRequestItemModelsAsync(PurchaseRequestItemModel purchaseRequestItemModel)
	{
		return await _itemModelRepository.AddAsync(purchaseRequestItemModel);
	}

	public async Task<List<PurchaseRequestItemModel>> GetItemRequestItemModelByIdAsync(int id)
	{
		return await _itemModelRepository.GetByConditionAsync(value => value.PurchaseRequestId == id);
	}
}
