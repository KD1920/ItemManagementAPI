using AutoMapper;
using ItemManagement.Data;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.Common.Service.Interface;
using ItemManagement.ApplicationConstants.Enums;
using ItemManagement.Domain.Models.RequestModels;
using ItemManagement.Domain.Models.ResponseModels;
using ItemManagement.Domain.Models.SearchParamModels;
using ItemManagement.Common.Helpers;

namespace ItemManagement.Services;

public class UserItemRequestService(
		IMapper _mapper,
		IUserItemRequestRepository repository,
		IUserItemRepository userItemRepository,
		IItemModelRepository itemModelRepository,
		ICommonService commonService,
		IGenericRepository<Status> genericRepository,
		IGenericRepository<User> userGenericRepository,
		IGenericRepository<ItemRequest> userItemGenericRepository
) : IUserItemRequestService
{
	public async Task<RecordTotalRecordResponseHelper<UserItemRequestResponseModel>> GetAllItemRequestsAsync(UserItemRequestSearchParams searchParams)
	{
		var isSearchParamsValid = searchParams.PageSize > 0 && searchParams.Page > 0;
		if (!isSearchParamsValid)
			throw new ArgumentException("Please enter valid PageSize or Page.");
		if (searchParams.UserId != 0)
		{
			var isUserIdExists = await userGenericRepository.ExistsAsync(x => x.Id == searchParams.UserId);
			if (!isUserIdExists)
				throw new ArgumentException("UserId does not exists");
		}
		if (searchParams.StatusId != 0)
		{
			var isStatusExists = await IsStatusExists(searchParams.StatusId);
			if (!isStatusExists)
				throw new ArgumentException("Status does not exists");
		}
		var user = await commonService.GetUser();
		var data = await repository.GetAllItemRequestsAsync(searchParams);
		var IRAutoMapper = new List<UserItemRequestResponseModel>();
		foreach (var itemRequest in data.Records)
		{
			var request = _mapper.Map<UserItemRequestResponseModel>(itemRequest);
			if (request.UserId == user.UserId || user.IsAdmin)
			{
				var itemRequestData = await repository.GetItemRequestItemModelByIdAsync(itemRequest.Id);
				if (itemRequestData.Count > 0)
				{
					foreach (var item in itemRequestData)
					{
						request.ItemModels.Add(_mapper.Map<PurchaseRequestItemResponseModel>(item));
					}
				}
				IRAutoMapper.Add(_mapper.Map<UserItemRequestResponseModel>(request));
			}
		}
		var response = new RecordTotalRecordResponseHelper<UserItemRequestResponseModel>();
        response.Records = IRAutoMapper;
        response.TotalRecords = data.TotalRecords;
		return response;
	}

	public async Task<UserItemRequestResponseModel> GetItemRequestByIdAsync(int id)
	{
		var user = await commonService.GetUser();
		var data = await repository.GetItemRequestByIdAsync(id);
		if (data is null)
			throw new KeyNotFoundException();
		if (user.UserId != data.UserId && !user.IsAdmin)
			throw new ArgumentException("You are not authorized to modify this item request.");
		var response = _mapper.Map<UserItemRequestResponseModel>(data);
		var itemModelData = await repository.GetItemRequestItemModelByIdAsync(data.Id);
		if (itemModelData.Count > 0)
		{
			foreach (var item in itemModelData)
			{
				response.ItemModels.Add(_mapper.Map<PurchaseRequestItemResponseModel>(item));
			}
		}
		return response;
	}

	public async Task<UserItemRequestResponseModel> AddItemRequestAsync(AddUserItemWithQuantityRequestModel itemRequest, int statusId, int userId)
	{
		var IRAutoMapper = _mapper.Map<ItemRequest>(itemRequest);
		IRAutoMapper.RequestNumber = await GenerateRequestNumberAsync();
		IRAutoMapper.StatusId = statusId;
		IRAutoMapper.UserId = userId;
		if (!(itemRequest.ItemModels.Count > 0))
			throw new ArgumentException("At least one item must be included in the request.");
		foreach (var item in itemRequest.ItemModels)
		{
			var isItemAvailable = await itemModelRepository.ItemModelQuantityAvailableAsync(item);
			if (!isItemAvailable)
				throw new ArgumentException("The requested quantity exceeds the available stock for Item Id" + item.ItemModelId);
			if (!(item.Quantity > 0))
				throw new ArgumentException("Quantity must be greater than zero.");
		}
		var data = await repository.AddItemRequestAsync(IRAutoMapper);
		var response = _mapper.Map<UserItemRequestResponseModel>(data);
		response.ItemModels = itemRequest.ItemModels;
		foreach (var item in itemRequest.ItemModels)
		{
			var itemModel = _mapper.Map<AddUserItemQuantityRequestModel>(item);
			itemModel.ItemRequestId = response.Id;
			var itemModelData = _mapper.Map<ItemRequestItemModel>(itemModel);
			await repository.AddItemRequestItemModelAsync(itemModelData);
		}
		return response;
	}

	public async Task<UserItemRequestResponseModel> UpdateItemRequestAsync(int id, UpdateUserItemWithQuantityRequestModel itemRequest)
	{
		var user = await commonService.GetUser();
		if (!user.IsAdmin && (itemRequest.StatusId == (int)UserItemRequestStatus.Rejected || itemRequest.StatusId == (int)UserItemRequestStatus.Approved))
			throw new ArgumentException("Developer cannot Approve/Reject Item request.");
		if (user.IsAdmin && itemRequest.StatusId == (int)UserItemRequestStatus.Cancelled)
			throw new ArgumentException("Admin cannot Cancel Item request.");
		if (user.IsAdmin && (itemRequest.StatusId == (int)UserItemRequestStatus.Draft || itemRequest.StatusId == (int)UserItemRequestStatus.Pending))
			throw new ArgumentException("Admin cannot create a Draft/Pending Item request.");
		var itemRequestData = await repository.GetItemRequestByIdAsync(id);
		if (user.UserId != itemRequestData.UserId && !user.IsAdmin)
			throw new ArgumentException("You are not authorized to modify this item request.");
		itemRequestData.Comment = itemRequest.Comment;
		var statusFromName = (UserItemRequestStatus)itemRequestData.StatusId;
		var statusToName = (UserItemRequestStatus)itemRequest.StatusId;
		if (itemRequestData.StatusId == (int)UserItemRequestStatus.Draft || (itemRequestData.StatusId == (int)UserItemRequestStatus.Pending && itemRequest.StatusId != (int)UserItemRequestStatus.Draft))
			itemRequestData.StatusId = itemRequest.StatusId;
		else
			throw new ArgumentException("Changing status from " + statusFromName + " to " + statusToName + " is not allowed.");
		if (!(itemRequest.ItemModels.Count > 0))
			throw new ArgumentException("At least one item must be included in the request.");
		foreach (var item in itemRequest.ItemModels)
		{
			var isItemAvailable = await itemModelRepository.ItemModelQuantityAvailableAsync(item);
			if (!isItemAvailable)
				throw new ArgumentException("The requested quantity exceeds the available stock for Item Id" + item.ItemModelId);
			if (!(item.Quantity > 0))
				throw new ArgumentException("Quantity must be greater than zero.");
		}
		var isStatusExists = await IsStatusExists(itemRequest.StatusId);
		if (!isStatusExists)
			throw new ArgumentException("Status does not exists");

		var data = await repository.UpdateItemRequestAsync(itemRequestData);
		var response = _mapper.Map<UserItemRequestResponseModel>(data);
		response.ItemModels = itemRequest.ItemModels;
		await repository.DeleteItemRequestItemModelAsync(id);
		foreach (var item in itemRequest.ItemModels)
		{
			var itemModel = _mapper.Map<AddUserItemQuantityRequestModel>(item);
			itemModel.ItemRequestId = response.Id;
			var itemModelData = _mapper.Map<ItemRequestItemModel>(itemModel);
			await repository.AddItemRequestItemModelAsync(itemModelData);
			if (itemRequestData.StatusId == (int)UserItemRequestStatus.Approved)
			{
				var userItemModelQuantity = _mapper.Map<UserItem>(itemModel);
				var itemModelQuantity = _mapper.Map<ItemModel>(itemModel);
				userItemModelQuantity.UserId = user.UserId;
				await userItemRepository.AddUserItemQuantityAsync(userItemModelQuantity);
				await itemModelRepository.DeductItemModelQuantityAsync(itemModelQuantity);
			}
		}
		return response;
	}

	public async Task<bool> IsStatusExists(int id)
	{
		var response = await genericRepository.ExistsAsync(x => x.Id == id);
		return response;
	}

	public async Task<string> GenerateRequestNumberAsync()
	{
		var now = DateTime.Now;
		string month = now.ToString("MMM");
		string year = now.Year.ToString();
		string prefix = $"REQ-{month}-{year}-";
		var search = new UserItemRequestSearchParams();
		var existingRequestNumbers = await repository.GetAllItemRequestsAsync(search);

		int maxSequence = 0;

		foreach (var invoice in existingRequestNumbers.Records)
		{
			string[] parts = invoice.RequestNumber.Split('-');
			if (parts.Length == 4 && int.TryParse(parts[3], out int sequence))
			{
				if (sequence > maxSequence)
					maxSequence = sequence;
			}
		}

		int nextSequence = maxSequence + 1;
		var invoiceNumber = $"{prefix}{nextSequence}";
		return invoiceNumber;
	}
}